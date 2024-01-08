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
    
    public partial class ScholarBankAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScholarBankAccount()
        {
            this.ScholarReceives = new HashSet<ScholarReceive>();
        }
    
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNameSurname { get; set; }
        public string BankAccountIban { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CretaionDate { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
    
        public virtual Login Login { get; set; }
        public virtual Login Login1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScholarReceive> ScholarReceives { get; set; }
    }
}
