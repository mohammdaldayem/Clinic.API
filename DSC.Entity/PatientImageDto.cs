using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Entity
{
   public class PatientImageDto
    {
        public Byte[] ImageData { set; get; }
        public string ImageName { set; get; }
        public DateTime ImageDate { set; get; }
    }
}
