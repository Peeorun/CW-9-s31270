using Microsoft.AspNetCore.Mvc;
using CW_9_s31270.DTOs;
using CW_9_s31270.Services;
using CW_9_s31270.Exceptions;

namespace CW_9_s31270.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescriptionController(IDbService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto prescriptionData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            
                var prescriptionId = await service.CreatePrescriptionAsync(prescriptionData);
                return CreatedAtAction(nameof(GetPatientDetails), 
                    new { patientId = prescriptionData.Patient.IdPatient ?? 0 }, 
                    new { PrescriptionId = prescriptionId });
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientDetails([FromRoute] int patientId)
        {
            try
            {
                var patientDetails = await service.GetPatientDetailsAsync(patientId);
                return Ok(patientDetails);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}

