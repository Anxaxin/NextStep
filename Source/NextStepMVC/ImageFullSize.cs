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
    
    public partial class ImageFullSize
    {
        public int Id { get; set; }
        public byte[] imagedata { get; set; }
        public string imagetype { get; set; }
    
        public virtual GalleryImage GalleryImage { get; set; }
    }
}