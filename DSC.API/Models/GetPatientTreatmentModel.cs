using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSC.API.Models
{
    public class GetPatientTreatmentModel
    {
        [Required]
        public int PatientID { set; get; }
        [Required]
        public int VisiteID { set; get; }
    }
}
