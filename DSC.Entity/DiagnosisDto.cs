using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
   public class DiagnosisDto
    {
        public int DiagnosisID { set; get; }
        public string DiagnosisName { set; get; }
        public string ParentDiagnosisID { set; get; }
        public bool IsDeleted { set; get; }
    }
}
