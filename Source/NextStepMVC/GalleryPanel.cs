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
    
    public partial class GalleryPanel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GalleryPanel()
        {
            this.GalleryImages = new HashSet<GalleryImage>();
        }
    
        public int P_Id { get; set; }
        public string Header { get; set; }
        public string Context { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GalleryImage> GalleryImages { get; set; }
        public virtual Panel Panel { get; set; }
    }
}
