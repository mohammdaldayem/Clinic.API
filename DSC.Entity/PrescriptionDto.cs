using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
   public class PrescriptionDto
    {
        public int PrescriptionID { set; get; }
        public string PrescriptionName { set; get; }
        public int ParentPrescriptionID { set; get; }
        public bool IsDeleted { set; get; }
    }
}
