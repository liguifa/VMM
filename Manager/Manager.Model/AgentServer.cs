//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manager.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AgentServer
    {
        public AgentServer()
        {
            this.VMs = new HashSet<VM>();
        }
    
        public System.Guid AgentServer_Id { get; set; }
        public string AgentServer_Address { get; set; }
        public int AgentServer_Port { get; set; }
        public string AgentServer_Key { get; set; }
        public bool AgentServer_IsDel { get; set; }
    
        public virtual ICollection<VM> VMs { get; set; }
    }
}
