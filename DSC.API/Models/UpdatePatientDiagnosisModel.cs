﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSC.API.Models
{
    public class UpdatePatientDiagnosisModel
    {
        [Required]
        public int PatientID { set; get; }
        [Required]
        public int VisiteID { set; get; }
        [Required]
        public string Diagnosis { set; get; }
        [Required]
        public DateTime ImageDate { set; get; }
        [Required]
        public DateTime ImageTime { set; get; }
        [Required]
        public string ImageNote { set; get; }
    }
}
