using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.DTOs.Patient;
using VitaPoint.Server.Entities;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Controllers
{
    public class PatientController : VPBaseController
    {
        private readonly IPatientService _patientService;
        private readonly IAccountService _accountService;
        private readonly ApplicationDbContext _context;

        public PatientController(IPatientService patientService, IAccountService accountService, ApplicationDbContext context)
        {
            _patientService = patientService;
            _accountService = accountService;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPatientData()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            Patient patient = await _patientService.GetPatientByUser(UserId);

            if (patient == null)
            {
                return NotFound(new { message = "Patient data could not be found in system." });
            }

            PatientDto dto = new PatientDto();
            dto.TransferData(patient);

            return Ok(dto);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePatientData([FromBody] UpdatePatientDto dto)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            AuthResult validationResult = await _accountService.ValidatePassword(UserId, dto.CurrentPassword);

            if (!validationResult.Success) return Unauthorized(new { message = validationResult.ErrorMessage });

            //Use transaction so changes are all or nothing
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //Change password if user entered a new password in client. Otherwise skip.
                if (!String.IsNullOrWhiteSpace(dto.NewPassword))
                {
                    AuthResult passwordChangeResult = await _accountService.UpdatePassword(UserId, dto.CurrentPassword, dto.NewPassword);

                    if (!passwordChangeResult.Success)
                    {
                        throw new Exception(passwordChangeResult.ErrorMessage);
                    }
                }

                Patient? updatedPatient = await _patientService.UpdatePatient(dto, UserId);

                if (updatedPatient == null)
                {
                    throw new Exception("Patient data could not be found in database");
                }

                await transaction.CommitAsync();

                return Ok(new { message = "Patient profile updated" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { message = ex.Message });
            }            
        }
    }
}
