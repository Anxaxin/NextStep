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
    
    public partial class PageSetting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PageSetting()
        {
            this.Panels = new HashSet<Panel>();
            this.AdminLogins = new HashSet<AdminLogin>();
            this.PanelStyles = new HashSet<PanelStyle>();
            this.PageSettingDomains = new HashSet<PageSettingDomain>();
        }
    
        public int Id { get; set; }
        public string PageName { get; set; }
        public string FooterText { get; set; }
        public int pageStyle { get; set; }
        public bool singlePage { get; set; }
        public string PrimaryPageUri { get; set; }
        public byte[] HeaderImage { get; set; }
        public string HeaderImageType { get; set; }
        public int HeaderSize { get; set; }
        public int AlignHeaderRight { get; set; }
        public string Culture { get; set; }
    
        public virtual SiteStyleSheet SiteStyleSheet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Panel> Panels { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminLogin> AdminLogins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PanelStyle> PanelStyles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PageSettingDomain> PageSettingDomains { get; set; }
    }
}
