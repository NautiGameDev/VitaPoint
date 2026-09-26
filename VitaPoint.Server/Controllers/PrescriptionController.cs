using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaPoint.Server.DTOs.Prescriptions;
using VitaPoint.Server.Helpers;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Controllers
{
    public class PrescriptionController : VPBaseController
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPrescriptions()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<Prescription> prescriptions = await _prescriptionService.GetPrescriptionsByUser(UserId);

            List<PrescriptionDto> dtoList = prescriptions.Select(p => p.ToPrescriptionDto()).ToList();

            return Ok(dtoList);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> RequestRefill([FromRoute] int id)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            var (refillSuccess, prescription) = await _prescriptionService.RequestRefill(id, UserId);

            if (prescription == null) return NotFound(new { Message = "Prescription could not be found for that user" });

            //This message returns if the update cannot be completed - IE no refills remaining, or refill status is not none/filled
            if (!refillSuccess) return BadRequest(new { Message = "Couldn't request refill. Please contact your doctor." });
            
            return Ok(new { Message = "Refill request successful." });
        }
    }
}
