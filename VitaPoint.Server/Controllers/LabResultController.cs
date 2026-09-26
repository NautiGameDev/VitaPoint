using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaPoint.Server.DTOs.LabResults;
using VitaPoint.Server.Helpers;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Controllers
{
    public class LabResultController : VPBaseController
    {
        private readonly ILabResultService _labResultService;

        public LabResultController(ILabResultService labResultService)
        {
            _labResultService = labResultService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLabResults()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<LabResult> labResults = await _labResultService.GetLabResultsByUser(UserId);
            
            List<LabResultMetaDto> dtoList = labResults.Select(lr => lr.ToLabResultMetaDto()).ToList();

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetLabResultById([FromRoute]int id)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            LabResult? result = await _labResultService.GetLabResultById(id, UserId);

            if (result == null) return NotFound(new { Message = "No lab result by that id" });

            return Ok(result.ToLabResultDto());
        }
    }
}
