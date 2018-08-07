using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSC.API.Models
{
    public class UpdateFinancialModel
    {
        [Required]
        public int FinancialID { set; get; }
        [Required]
        public DateTime Financialdate { set; get; }
        [Required]
        public int TreatmentCost { set; get; }
        [Required]
        public int PatientCredit { set; get; }
        [Required]
        public int PatientDebit { set; get; }
    }
}
