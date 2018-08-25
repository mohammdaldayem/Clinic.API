using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSC.API.Models;
using DSC.Datalayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSC.API.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    public class PatientController : Controller
    {
        private readonly Patient _PatientDL;
        public PatientController()
        {
            _PatientDL = new Patient();
        }
        [HttpPost]
        //[Route("/GetPatientInfo")]
        public async Task<IActionResult> GetPatientInfo([FromBody] PatientInfoModel value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var list = await _PatientDL.GetPatientInfo(value.FromDate, value.ToDate, value.EmployeeID, value.PatientName, value.PatientID);
            return Ok(list);
        }
        [HttpGet]
        [Route("GetPatientVisits/{PatientID}")]
        public async Task<IActionResult> GetPatientVisits([FromRoute] int PatientID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var list = await _PatientDL.GetPatientVisits(PatientID);
            return Ok(list);
        }
        [HttpGet]
        [Route("GetPatientImages/{PatientID}")]
        public async Task<IActionResult> GetPatientImages([FromRoute] GetPatientImagesModel Patient)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var list = await _PatientDL.GetPatientImages(Patient.PatientID);
            return Ok(list);
        }
        [HttpPost]
        [Route("InseartPatientImage/{Patient}")]
        public async Task<IActionResult> InseartPatientImage([FromRoute] InseartPatientImageModel Patient)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var Success = await _PatientDL.InseartPatientImage(Patient.PatientID, Patient.ImageData, Patient.ImageName, Patient.Note);
            if (Success > 0)
                return Ok();
            else
                return BadRequest();

        }

        #region Treatment
        [HttpGet]
        [Route("GetPatientTreatment/{PatientID}/{VisitID}")]
        public async Task<IActionResult> GetPatientTreatment(int PatientID,int VisitID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            string Tratment = await _PatientDL.GetPatientTreatment(PatientID, VisitID);
            return Ok(Tratment);
        }
        [HttpPost]
        [Route("InseartPatientImage/{Treatment}")]
        public async Task<IActionResult> UpdatePatientTreatment([FromRoute] UpdatePatientTreatmentModel Treatment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var Success = await _PatientDL.UpdatePatientTreatment(Treatment.PatientID, Treatment.VisiteID, Treatment.Treatmen, Treatment.TreatmenText);
            if (Success > 0)
                return Ok();
            else
                return BadRequest();

        }
        #endregion

    }
}