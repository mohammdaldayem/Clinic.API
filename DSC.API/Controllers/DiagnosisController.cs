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
    public class DiagnosisController : Controller
    {
        private readonly Diagnosis _diagnosisDL;
        public DiagnosisController ()
        {
            _diagnosisDL = new Diagnosis();
        }
        [HttpGet]
        [Route ("GetTreatment")]
        public async Task<IActionResult> GetTreatment()
        {
            if (!ModelState.IsValid) return BadRequest (ModelState);
            var DiagnosisParents = await _diagnosisDL.LoadDiagnosisParents(false);
            foreach (var _diagnosisParent in DiagnosisParents)
            {
                _diagnosisParent.Diagnosis = await _diagnosisDL.LoadDiagnosis (_diagnosisParent.ParentOperationID, false);
            }
            return Ok (DiagnosisParents);
        }
    }
}