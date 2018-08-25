using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
   public class DiagnosisParentDto
    {
        public string ParentName { set; get; }
        public int ParentOperationID { set; get; }

        public List<DiagnosisDto> Diagnosis { set; get; }
    }
}
