using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSC.API.Models
{
    public class InseartPatientImageModel
    {
        [Required]
        public int PatientID { set; get; }
        [Required]
        public Byte[] ImageData { set; get; }
        [Required]
        public string ImageName { set; get; }
        public string Note { set; get; }
    }
}
