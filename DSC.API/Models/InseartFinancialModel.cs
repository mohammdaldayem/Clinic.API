using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSC.API.Models
{
    public class InseartFinancialModel
    {
        [Required]
        public DateTime Financialdate { set; get; }
        [Required]
        public int TreatmentCost { set; get; }
        [Required]
        public int PatientCredit { set; get; }
        [Required]
        public int PatientDebit { set; get; }
        public string Note { set; get; }
        [Required]
        public int VisitID { set; get; }
        [Required]
        public int EmployeeID { set; get; }
        [Required]
        public int PatientID { set; get; }
        [Required]
        public int CreatedBy { set; get; }
        [Required]
        public DateTime CreationDate { set; get; }
        [Required]
        public int PaymentTypeID { set; get; }
    }
}
