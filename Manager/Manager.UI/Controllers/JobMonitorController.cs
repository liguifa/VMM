using Manager.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.UI.Controllers
{
    public class JobMonitorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.jobs = new Job().GetJobs("1", "10");
            return View();
        }
    }
}