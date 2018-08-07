using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSC.Datalayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSC.API.Controllers
{

    [Route("api/[controller]")]
    public class TreatmentController : Controller
    {
        private readonly Treatment _treatmentDL;
        public TreatmentController()
        {
            _treatmentDL = new Treatment();
        }

        [HttpGet]
        [Route("GetTreatment")]
        public async Task<IActionResult> GetTreatment()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var TreatmentParents = await _treatmentDL.LoadTreatmentParents(false);
            foreach (var _treatmentParent in TreatmentParents)
            {
                _treatmentParent.Treatments = await _treatmentDL.LoadTreatments(_treatmentParent.ParentOperationID, false);
            }
            return Ok(TreatmentParents);
        }
    }
}