using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
   public class PrescriptionParentDto
    {
        public string ParentName { set; get; }
        public int ParentOperationID { set; get; }

        public List<PrescriptionDto> Prescriptions { set; get; }
    }

    
}
