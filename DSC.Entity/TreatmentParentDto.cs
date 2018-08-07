using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
    public class TreatmentParentDto
    {
        public string ParentName { set; get; }
        public int ParentOperationID { set; get; }

        public List<TreatmentDto> Treatments { set; get; }
    }
}
