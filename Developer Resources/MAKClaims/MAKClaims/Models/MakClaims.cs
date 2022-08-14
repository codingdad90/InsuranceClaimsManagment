using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MAKClaims.Models
{
    [MetadataType(typeof(ClaimMetadata))]

    public partial class Claim
    {
        private sealed class ClaimMetadata
        {
            [Display(Name ="Claim Number")]
            public int ClaimId { get; set; }
            [Display(Name = "Date of Loss")]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime DateOfLoss { get; set; }
            [DataType(DataType.Currency)]
            public decimal Reserves { get; set; }
            [DataType(DataType.Currency)]
            public decimal Deductable { get; set; }
            [DataType(DataType.Currency)]
            public decimal AmountPaid { get; set; }


        }

    }
    public partial class Insured
    {
        public string FullName
        {
            get
            {
                return Name_First + " " + Name_Last;
            }
        }
    }


    [MetadataType(typeof(AdjustorMetadata))]
    public partial class Adjustor
    {
        private sealed class AdjustorMetadata
        {
            [Display(Name = "Adjustor")]
            public string Name { get; set; }          
        }
        [Display(Name = "Active Claims")]
        public int TotalClaim
        {
            get
            {              
                return Claims.Where(x => x.Status == true).Count();
            }

        }
        [Display(Name = "Total Reserves")]
        [DataType(DataType.Currency)]
        public double TotalReserve
        {
            get
            {
                decimal d = Claims.Sum(r => r.Reserves);
                return (double)d;
            }
        }
        [Display(Name = "Total Deductable")]
        [DataType(DataType.Currency)]
        public double TotalDeductable
        {
            get
            {
                decimal d = Claims.Sum(r => r.Deductable);
                return (double)d;
            }
        }
        [Display(Name = "Amount Paid")]
        [DataType(DataType.Currency)]
        public double TotalAmoutPaid
        {
            get
            {
                decimal d = Claims.Sum(r => r.AmountPaid);
                return (double)d;
            }
        }

    }
    public partial class ClaimActions
    {
        private sealed class ClaimactionsMetadata
        {
            
            [Display(Name = "Date")]
            
            [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM/dd/yyyy}")]
            [DataType(DataType.Date)]
            public DateTime Date { get; set; }
            [DataType(DataType.Currency)]
            public decimal DollarAmount { get; set; }
        }

    }

}
