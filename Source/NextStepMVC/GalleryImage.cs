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
    
    public partial class GalleryImage
    {
        public int ID { get; set; }
        public int Gallery_Id { get; set; }
        public string imageType { get; set; }
        public byte[] imageThumb { get; set; }
        public string imageTitle { get; set; }
        public string imageDescription { get; set; }
    
        public virtual GalleryPanel GalleryPanel { get; set; }
        public virtual ImageFullSize ImageFullSize { get; set; }
    }
}