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
    
    public partial class ClaimAction
    {
        public int ClaimActionId { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<decimal> DollarAmount { get; set; }
        public string Note { get; set; }
        public int ClaimId { get; set; }
        public int ActionId { get; set; }
        public int AdjustorId { get; set; }
        public Nullable<bool> Complete { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual Adjustor Adjustor { get; set; }
        public virtual Claim Claim { get; set; }
    }
}
