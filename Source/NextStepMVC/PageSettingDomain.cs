//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NextStepMVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class PageSettingDomain
    {
        public int Id { get; set; }
        public int PageSettingId { get; set; }
        public string Domain { get; set; }
        public string Culture { get; set; }
        public bool NextStepAlias { get; set; }
    
        public virtual PageSetting PageSetting { get; set; }
    }
}