using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
   public class VisitDto
    {
        //[Field("VisitID")]
        public int VisitID { set; get; }
        public int PatientID { set; get; }
        public string Treatment { set; get; }
        public string diagnosis { set; get; }

        public DateTime VisitTime { get; set; }
        public DateTime VisitDate { get; set; }
        
    }
}
