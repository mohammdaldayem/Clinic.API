using System;

namespace DSC.Entity
{
    public class PatientDto
    {
        public string PatientName { set; get; } 
        public int PatientID { set; get; }
        public int PatientNo { set; get; }
        public string Gender { set; get; }
        public string MobileNumber { set; get; }
        public string mobilenumber2 { set; get; }
        public int Age { set; get; }
        public DateTime CreationDate { set; get; }
        public string DrName { set; get; }
        public DateTime? FromDate { set; get; }
        public DateTime? ToDate { set; get; }
    }
}
