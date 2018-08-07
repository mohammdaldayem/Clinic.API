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
    [Route("api/[controller]")]
    public class FinancialController : Controller
    {
        private readonly Financial _FinancialDL;
        public FinancialController()
        {
            _FinancialDL = new Financial();
        }
        [HttpGet]
        [Route("GetPatientFinancial/{PatientID}/{EmployeeID}")]
        public async Task<IActionResult> GetPatientFinancial([FromRoute] int PatientID,int EmployeeID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var list = await _FinancialDL.GetPatientFinancial(PatientID, EmployeeID);
            return Ok(list);
        }
        [HttpPost]
        [Route("InseartPatientFinancial")]
        public async Task<IActionResult> InseartPatientFinancial([FromBody] InseartFinancialModel Financialobj)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var Success = await _FinancialDL.InseartPatientFinancial(Financialobj.TreatmentCost, Financialobj.PatientCredit, Financialobj.PatientDebit, Financialobj.Financialdate, Financialobj.Note, Financialobj.VisitID, Financialobj.EmployeeID, Financialobj.PatientID, Financialobj.CreatedBy, Financialobj.CreationDate, Financialobj.PaymentTypeID);
            if (Success == -1)
                return Ok();
            else
                return BadRequest();
        }
        [HttpPost]
        [Route("UpdatePatientFinancial")]
        public async Task<IActionResult> UpdatePatientFinancial([FromBody] UpdateFinancialModel Financialobj)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var Success = await _FinancialDL.UpdatePatientFinancial(Financialobj.FinancialID,Financialobj.TreatmentCost, Financialobj.PatientCredit, Financialobj.PatientDebit, Financialobj.Financialdate);
            if (Success > 0)
                return Ok();
            else
                return BadRequest();
        }
    }
}