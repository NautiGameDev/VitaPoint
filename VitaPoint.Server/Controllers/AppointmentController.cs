using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaPoint.Server.DTOs.Appointment;
using VitaPoint.Server.Helpers;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Controllers
{
    public class AppointmentController : VPBaseController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientDoctorService _patientDoctorService;      
        
        public AppointmentController(IAppointmentService appointmentService, IPatientDoctorService patientDoctorService)
        {
            _appointmentService = appointmentService;
            _patientDoctorService = patientDoctorService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAppointmentsForUser()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch appointment data" });

            List<Appointment> appointments = await _appointmentService.GetAppointmentsForUser(UserId);

            return Ok(appointments.Select(a => a.ToAppointmentDto()));
        }


        //Returns a list of doctors the user is able to schedule appointments with
        [HttpGet("available-doctors")]
        [Authorize]
        public async Task<IActionResult> GetAvailableDoctorsForUser()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);

            return Ok(pdList.Select(pd => pd.ToDoctorReceipientDto()));
        }

        //Returns list of appointments that are currently scheduled for requested date/doctor
        [HttpGet("unavailable-appointments")]
        [Authorize]
        public async Task<IActionResult> GetUnavailableAppointments([FromQuery] string doctorId, DateOnly date)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);
            PatientDoctor? verifiedMatch = pdList.FirstOrDefault(pd => pd.PatientUserId == UserId && pd.DoctorUserId == doctorId);
            if (verifiedMatch == null) return StatusCode(403, new { message = "User unauthorized to see available appointments for that doctor" });

            List<Appointment> currentAppointments = await _appointmentService.GetCurrentAppointments(doctorId, date);

            return Ok(currentAppointments.Select(a => a.ToUnavailableAppointmentDto()));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewAppointment([FromBody] NewAppointmentDto dto)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to access this endpoint" });

            //Test if requested time slot is valid
            if (!AppointmentRules.ValidTimeSlots.Contains(dto.TimeSlot)) return BadRequest(new { message = "Time slot requested not a valid selection" });


            //Test if user can schedule appointments with requested doctor
            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);
            PatientDoctor? verifiedMatch = pdList.FirstOrDefault(pd => pd.PatientUserId == UserId && pd.DoctorUserId == dto.DoctorId);
            if (verifiedMatch == null) return StatusCode(StatusCodes.Status403Forbidden, new { message = "User not authorized to schedule appointment with that doctor" });

            //Test if appointment already exists
            if (await _appointmentService.DoesAppointmentExist(dto.DoctorId, dto.Date, dto.TimeSlot)) return Conflict(new { message = $"Appointment already exists at {dto.Date} {dto.TimeSlot}" });
            
            //Create new appointment if checks are passed
            Appointment? newAppointment = await _appointmentService.NewAppointment(dto, verifiedMatch.Patient, verifiedMatch.Doctor);
            if (newAppointment == null) return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to create new appointment." });
            return Ok(new { message = $"New appointment created at {dto.Date} {dto.TimeSlot}" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to access this endpoint" });

            Appointment? cancelledAppointment = await _appointmentService.CancelAppointment(id, UserId);
            if (cancelledAppointment == null) return NotFound(new { message = "Appointment not found or access denied." });

            return Ok(new { message = "Appointment successfully cancelled" });
        }
    }
}
