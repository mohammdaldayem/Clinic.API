using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSC.Datalayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSC.API.Controllers
{
    [Route ("api/[controller]")]
    public class PrescriptionController : Controller
    {
        private readonly Prescription _prescriptionDL;
        public PrescriptionController ()
        {
            _prescriptionDL = new Prescription ();
        }
        [HttpGet]
        [Route ("GetPrescriptions")]
        public async Task<IActionResult> GetPrescriptions ()
        {
            if (!ModelState.IsValid) return BadRequest (ModelState);
            var PrescriptionParents = await _prescriptionDL.LoadPrescriptionParents (false);
            foreach (var _prescriptionParents in PrescriptionParents)
            {
                _prescriptionParents.Prescriptions = await _prescriptionDL.LoadPrescription (_prescriptionParents.ParentOperationID, false);
            }
            return Ok (PrescriptionParents);
        }
    }
}