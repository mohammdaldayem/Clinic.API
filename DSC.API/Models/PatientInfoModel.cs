using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSC.API.Models
{
    public class PatientInfoModel
    {
        public DateTime? FromDate { set; get; } 
        public DateTime? ToDate { set; get; } 
        public int? EmployeeID { set; get; } 
        public string PatientName { set; get; } 
        public int? PatientID { set; get; } 
    }
}
