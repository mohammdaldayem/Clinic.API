using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
    public class FinancialDto
    {
        public int Visitid { set; get; }
        public int FinancialID { set; get; }
        public string DoctorName { set; get; }
        public int EmployeeID { set; get; }
        public DateTime FinancialDate { set; get; }
        public string Note { set; get; }
        public int TreatmentCost { set; get; }
        public int PatientCredit { set; get; }
        public int PatientDebit { set; get; }
        public bool IsDoctor { set; get; }
    }
}
