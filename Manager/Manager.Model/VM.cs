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
    
    public partial class VM
    {
        public System.Guid VM_Id { get; set; }
        public string VM_Name { get; set; }
        public string VM_System { get; set; }
        public System.Guid VM_User { get; set; }
        public System.Guid VM_CreateUser { get; set; }
        public System.DateTime VM_CreateTime { get; set; }
        public System.DateTime VM_OperationTime { get; set; }
        public bool VM_IsDel { get; set; }
        public System.Guid VM_Agent { get; set; }
    
        public virtual AgentServer AgentServer { get; set; }
        public virtual User User { get; set; }
    }
}
