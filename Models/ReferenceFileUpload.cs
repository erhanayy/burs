//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAdmin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReferenceFileUpload
    {
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public int FileTypeId { get; set; }
        public string UplodedFileData { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    
        public virtual FileType FileType { get; set; }
        public virtual Login Login { get; set; }
        public virtual Login Login1 { get; set; }
        public virtual Reference Reference { get; set; }
    }
}