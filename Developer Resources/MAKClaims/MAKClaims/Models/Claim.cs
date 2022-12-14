//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MAKClaims.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Claim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Claim()
        {
            this.ClaimActions = new HashSet<ClaimAction>();
            this.Attachments = new HashSet<Attachment>();
            Status = true;
            Reserves = 5000;
            DateOfLoss = DateTime.Now;
        }
    
        public int ClaimId { get; set; }
        public int AdjustorId { get; set; }
        public int PropertyId { get; set; }
        public System.DateTime DateOfLoss { get; set; }
        public byte[] Attachment { get; set; }
        public decimal Reserves { get; set; }
        public decimal Deductable { get; set; }
        public decimal AmountPaid { get; set; }
        [DefaultValue(true)]
        public bool Status { get; set; }
    
        public virtual Adjustor Adjustor { get; set; }
        public virtual Property Property { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClaimAction> ClaimActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
