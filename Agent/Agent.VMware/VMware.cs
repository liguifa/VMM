using Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vim25Api;

namespace Agent.VMware
{
    public class VMware
    {
        private readonly VimService mService;
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Address { get; private set; }
        private ServiceContent mContext;

        private void Login()
        {
            var svcRef = new ManagedObjectReference();
            svcRef.type = "ServiceInstance";
            svcRef.Value = "ServiceInstance";
            this.mService.Url = String.Format("https://{0}/sdk", this.Address);
            this.mService.Timeout = 1800000;
            this.mService.CookieContainer = new System.Net.CookieContainer();
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            this.mContext = this.mService.RetrieveServiceContent(svcRef);
            this.mService.Login(this.mContext.sessionManager, this.Username, this.Password, null);
        }

        public VMware(string address, string username, string password)
        {
            this.mService = new VimService();
            this.Address = address;
            this.Username = username;
            this.Password = password;
            this.Login();
        }

        public List<SystemInfoResponseMessage.VMSystem> GetVMSystems()
        {
            ManagedObjectReference usecoll = this.mContext.propertyCollector;
            PropertyFilterSpec spec = new PropertyFilterSpec();
            spec.propSet = new PropertySpec[] { new PropertySpec() };
            spec.propSet[0].type = "Folder";
            spec.propSet[0].all = true;
            spec.objectSet = new ObjectSpec[] { new ObjectSpec() };
            spec.objectSet[0].obj = this.mContext.rootFolder;
            spec.objectSet[0].skip = true;
            RetrieveResult datacenter = this.mService.RetrievePropertiesEx(usecoll, new PropertyFilterSpec[] { spec }, new RetrieveOptions() { maxObjects = 0, maxObjectsSpecified = false });
            if (datacenter.objects == null)
            {
                return null;
            }

            TraversalSpec tSpec = GetVmTraversalSpec();
            PropertySpec propertySpec = new PropertySpec();
            propertySpec.all = false;
            propertySpec.pathSet = new string[] { "name" };
            propertySpec.type = "VirtualMachine";

            ObjectSpec objectSpec = new ObjectSpec();
            objectSpec.obj = this.mContext.rootFolder;
            objectSpec.skip = true;
            objectSpec.selectSet = new SelectionSpec[] { tSpec };

            PropertyFilterSpec propertyFilterSpec = new PropertyFilterSpec();
            propertyFilterSpec.propSet = new PropertySpec[] { propertySpec };
            propertyFilterSpec.objectSet = new ObjectSpec[] { objectSpec };

            List<PropertyFilterSpec> listpfs = new List<PropertyFilterSpec>(1);
            listpfs.Add(propertyFilterSpec);

            RetrieveResult listobjcont = this.mService.RetrievePropertiesEx(this.mContext.propertyCollector, listpfs.ToArray(), new RetrieveOptions() { maxObjects = 0, maxObjectsSpecified = false });
            List<SystemInfoResponseMessage.VMSystem> systems = new List<SystemInfoResponseMessage.VMSystem>();
            if (listobjcont != null)
            {
                foreach (ObjectContent oc in listobjcont.objects)
                {
                    String vmnm = null;
                    List<DynamicProperty> dps = oc.propSet.ToList();
                    if (dps != null)
                    {
                        vmnm = (String)dps[0].val;
                        if (vmnm != null)
                        {
                            SystemInfoResponseMessage.VMSystem vm = new SystemInfoResponseMessage.VMSystem()
                            {
                                Name = (String)dps[0].val,
                                Status = this.GetVmCpuEntitlementByVmName((String)dps[0].val).ToString(),
                                CreateTime = "",
                                LastOperationTime = ""
                            };
                            systems.Add(vm);
                        }

                    }
                }
            }
            return systems;
        }

        public double GetVmCpuEntitlementByVmName(String VmName)
        {
            double ans = 0;
            List<List<long>> list = GetVmData(VmName, "entitlement", "cpu");
            long maxInner = 0;
            int times = 0;
            foreach (List<long> listOuter in list)
            {
                long tempInner = 0;
                foreach (long inner in listOuter)
                {
                    tempInner += inner;
                }
                if (tempInner > maxInner)
                {
                    maxInner = tempInner;
                    times = listOuter.Count();
                }
            }
            if (times != 0)
            {
                ans = (double)maxInner / times;
            }

            return ans;
        }

        private static TraversalSpec GetVmTraversalSpec()
        {
            TraversalSpec vAppToVM = new TraversalSpec();
            vAppToVM.name = "vAppToVM";
            vAppToVM.type = "VirtualApp";
            vAppToVM.path = "vm";

            TraversalSpec vAppToVApp = new TraversalSpec();
            vAppToVApp.name = "vAppToVApp";
            vAppToVApp.type = "VirtualApp";
            vAppToVApp.path = "resourcePool";

            SelectionSpec vAppRecursion = new SelectionSpec();
            vAppRecursion.name = "vAppToVApp";
            SelectionSpec vmInVApp = new SelectionSpec();
            vmInVApp.name = "vAppToVM";
            List<SelectionSpec> vAppToVMSS = new List<SelectionSpec>();
            vAppToVMSS.Add(vAppRecursion);
            vAppToVMSS.Add(vmInVApp);
            vAppToVApp.selectSet = vAppToVMSS.ToArray();

            SelectionSpec sSpec = new SelectionSpec();
            sSpec.name = "VisitFolders";

            TraversalSpec dataCenterToVMFolder = new TraversalSpec();
            dataCenterToVMFolder.name = "DataCenterToVMFolder";
            dataCenterToVMFolder.type = "Datacenter";
            dataCenterToVMFolder.path = "vmFolder";
            dataCenterToVMFolder.skip = false;
            dataCenterToVMFolder.selectSet = new SelectionSpec[] { sSpec };

            TraversalSpec traversalSpec = new TraversalSpec();
            traversalSpec.name = "VisitFolders";
            traversalSpec.type = "Folder";
            traversalSpec.path = "childEntity";
            traversalSpec.skip = false;
            List<SelectionSpec> sSpecArr = new List<SelectionSpec>();
            sSpecArr.Add(sSpec);
            sSpecArr.Add(dataCenterToVMFolder);
            sSpecArr.Add(vAppToVM);
            sSpecArr.Add(vAppToVApp);
            traversalSpec.selectSet = sSpecArr.ToArray();
            return traversalSpec;
        }


        private List<ObjectContent> RetrievePropertiesAllObjects(List<PropertyFilterSpec> listpfs)
        {
            RetrieveOptions propObjectRetrieveOpts = new RetrieveOptions();
            List<ObjectContent> listobjcontent = new List<ObjectContent>();

            try
            {
                RetrieveResult rslts = this.mService.RetrievePropertiesEx(this.mContext.propertyCollector, listpfs.ToArray(), propObjectRetrieveOpts);
                if (rslts != null && rslts.objects != null && rslts.objects.Count() > 0)
                {
                    listobjcontent = rslts.objects.ToList();
                }
                String token = null;
                if (rslts != null && rslts.token != null)
                {
                    token = rslts.token;
                }
                while (token != null && token.Count() > 0)
                {
                    rslts = this.mService.ContinueRetrievePropertiesEx(this.mContext.propertyCollector, token);
                    token = null;
                    if (rslts != null)
                    {
                        token = rslts.token;
                        if (rslts.objects != null && rslts.objects.Count() > 0)
                        {
                            listobjcontent = listobjcontent.Concat(rslts.objects.ToList()).ToList();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return listobjcontent;
        }


        private ManagedObjectReference GetVmByVmName(String vmName)
        {
            ManagedObjectReference retVal = null;
            ManagedObjectReference rootFolder = this.mContext.rootFolder;
            try
            {
                TraversalSpec tSpec = GetVmTraversalSpec();
                PropertySpec propertySpec = new PropertySpec();
                propertySpec.all = false;
                propertySpec.pathSet = new string[] { "name" };
                propertySpec.type = "VirtualMachine";

                ObjectSpec objectSpec = new ObjectSpec();
                objectSpec.obj = this.mContext.rootFolder;
                objectSpec.skip = true;
                objectSpec.selectSet = new SelectionSpec[] { tSpec };

                PropertyFilterSpec propertyFilterSpec = new PropertyFilterSpec();
                propertyFilterSpec.propSet = new PropertySpec[] { propertySpec };
                propertyFilterSpec.objectSet = new ObjectSpec[] { objectSpec };

                List<PropertyFilterSpec> listpfs = new List<PropertyFilterSpec>(1);
                listpfs.Add(propertyFilterSpec);
                List<ObjectContent> listobjcont = RetrievePropertiesAllObjects(listpfs);

                if (listobjcont != null)
                {
                    foreach (ObjectContent oc in listobjcont)
                    {
                        ManagedObjectReference mr = oc.obj;
                        String vmnm = null;
                        List<DynamicProperty> dps = oc.propSet.ToList();
                        if (dps != null)
                        {
                            foreach (DynamicProperty dp in dps)
                            {
                                vmnm = (String)dp.val;
                            }
                        }
                        if (vmnm != null && vmnm.Equals(vmName))
                        {
                            retVal = mr;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return retVal;
        }
        private List<PerfCounterInfo> GetPerfCounters()
        {
            List<PerfCounterInfo> pciArr = null;

            try
            {
                PropertySpec propertySpec = new PropertySpec();
                propertySpec.all = false;
                propertySpec.pathSet = new string[] { "perfCounter" };
                propertySpec.type = "PerformanceManager";
                List<PropertySpec> propertySpecs = new List<PropertySpec>();
                propertySpecs.Add(propertySpec);

                ObjectSpec objectSpec = new ObjectSpec();
                objectSpec.obj = this.mContext.perfManager;
                List<ObjectSpec> objectSpecs = new List<ObjectSpec>();
                objectSpecs.Add(objectSpec);

                PropertyFilterSpec propertyFilterSpec = new PropertyFilterSpec();
                propertyFilterSpec.propSet = new PropertySpec[] { propertySpec };
                propertyFilterSpec.objectSet = new ObjectSpec[] { objectSpec };

                List<PropertyFilterSpec> propertyFilterSpecs = new List<PropertyFilterSpec>();
                propertyFilterSpecs.Add(propertyFilterSpec);

                List<PropertyFilterSpec> listpfs = new List<PropertyFilterSpec>(10);
                listpfs.Add(propertyFilterSpec);
                List<ObjectContent> listobjcont = RetrievePropertiesAllObjects(listpfs);

                if (listobjcont != null)
                {
                    foreach (ObjectContent oc in listobjcont)
                    {
                        List<DynamicProperty> dps = oc.propSet.ToList();
                        if (dps != null)
                        {
                            foreach (DynamicProperty dp in dps)
                            {

                                List<PerfCounterInfo> pcinfolist = ((XElement)(dp.val)).Elements("PerfCounterInfo").Select(d => d.Value as PerfCounterInfo).ToList();
                                pciArr = pcinfolist;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return pciArr;
        }

        private List<List<long>> GetVmData(String vmName, String nameInfo, String groupInfo)
        {
            List<List<long>> list = new List<List<long>>();
            ManagedObjectReference vmmor = GetVmByVmName(vmName);
            if (vmmor != null)
            {
                List<PerfCounterInfo> cInfo = GetPerfCounters();

                int i = 0;
                Dictionary<long, PerfCounterInfo> counters = new Dictionary<long, PerfCounterInfo>();
                foreach (PerfCounterInfo p in cInfo)
                {
                    counters.Add(Convert.ToInt64(p.key), p);
                }

                List<PerfMetricId> listpermeid = this.mService.QueryAvailablePerfMetric(this.mContext.perfManager, vmmor, DateTime.Now, false, DateTime.Now, false, 20, false).ToList();
                List<PerfMetricId> mMetrics = new List<PerfMetricId>();
                if (listpermeid != null)
                {
                    for (int index = 0; index < listpermeid.Count(); ++index)
                    {
                        if (counters.ContainsKey(Convert.ToInt64(listpermeid[index].counterId)))
                        {
                            mMetrics.Add(listpermeid[index]);
                        }
                    }
                }
                PerfQuerySpec qSpec = new PerfQuerySpec();
                qSpec.entity = vmmor;
                qSpec.maxSample = 10;
                qSpec.metricId = mMetrics.ToArray();
                qSpec.intervalId = 20;

                List<PerfQuerySpec> qSpecs = new List<PerfQuerySpec>();
                qSpecs.Add(qSpec);

                List<PerfEntityMetricBase> listpemb = this.mService.QueryPerf(this.mContext.perfManager, qSpecs.ToArray()).ToList();
                List<PerfEntityMetricBase> pValues = listpemb;
                for (i = 0; i < pValues.Count(); i++)
                {
                    List<PerfMetricSeries> listpems = ((PerfEntityMetric)pValues[i]).value.ToList();
                    List<PerfSampleInfo> listinfo = ((PerfEntityMetric)pValues[i]).sampleInfo.ToList();
                    for (int vi = 0; vi < listpems.Count(); ++vi)
                    {
                        String printInf = "";
                        PerfCounterInfo pci = (PerfCounterInfo)counters[Convert.ToInt64(listpems[vi].id.counterId)];

                        if (pci != null)
                        {
                            if (pci.nameInfo.key.Equals(nameInfo) && pci.groupInfo.key.Equals(groupInfo))
                            {
                                printInf += vi + ":" + pci.nameInfo.summary + ":" + pci.nameInfo.key + ":" + pci.nameInfo.label + ":"
                                        + pci.groupInfo.key + ":" + pci.groupInfo.label + ":" + pci.groupInfo.summary + " ";

                                foreach (PerfMetricId pmi in mMetrics)
                                {
                                    int counterId = pmi.counterId;
                                    if (counterId == listpems[vi].id.counterId)
                                    {
                                        printInf += "[" + pmi.counterId + ":" + pmi.instance + "]    ";
                                    }
                                }

                                if (listpems[vi] is PerfMetricIntSeries)
                                {
                                    PerfMetricIntSeries val = (PerfMetricIntSeries)listpems[vi];
                                    List<long> lislon = val.value.ToList();
                                    foreach (long k in lislon)
                                    {
                                        printInf += k + " ";
                                    }
                                    list.Add(lislon);
                                }
                                printInf += "   " + pci.unitInfo.key + " " + pci.unitInfo.label + " " + pci.unitInfo.summary;
                            }
                        }
                    }
                }

            }

            return list;
        }
    }
}
