using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaPoint.Server.DTOs.Messages;
using VitaPoint.Server.Helpers;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Controllers
{
    public class MessageController : VPBaseController
    {
        private readonly IMessageService _messageservice;
        private readonly IPatientDoctorService _patientDoctorService;

        public MessageController(IMessageService messageservice, IPatientDoctorService patientDoctorService)
        {
            _messageservice = messageservice;
            _patientDoctorService = patientDoctorService;
        }

        [HttpGet("root_messages")]
        [Authorize]
        public async Task<IActionResult> GetRootMessages()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<Message> messages = await _messageservice.GetRootMessagesForUser(UserId);

            if (messages.Count == 0) return NotFound(new { message = "No messages found for user" });

            //Gets full list of patient-doctor connections by userid. Used to retrieve names of senders/receivers when building message DTOs
            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);

            List<MessageDto> dtoList = MapMessagesToDto(pdList, messages);           

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMessageThread([FromRoute] int id)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<Message> messages = await _messageservice.GetRepliesByRootId(UserId, id);

            if (messages.Count == 0) return NotFound(new { message = "No messages found for thread" });

            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);

            List<MessageDto> dtoList = MapMessagesToDto(pdList, messages);

            return Ok(dtoList);
        }

        //Should only be called from patient client. Method filters PatientDoctor data according to patient id
        [HttpGet("get-contacts")]
        [Authorize]
        public async Task<IActionResult> GetContactsForPatient()
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);
            

            List<RecipientDto> recipients = pdList
                .Where(pd => pd.PatientUserId == UserId)
                .Select(pd => pd.ToDoctorReceipientDto()).ToList();

            if (recipients.Count == 0) return NotFound(new { message = "No contacts found for user" });

            return Ok(recipients);            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewMessage([FromBody] NewMessageDto dto)
        {
            if (string.IsNullOrEmpty(UserId)) return Unauthorized(new { message = "User not authorized to fetch data" });

            string senderId = UserId;

            //Take user id from thread that doesn't match id of logged in user
            string receiverId = dto.UserIds[0] == UserId ? dto.UserIds[1] : dto.UserIds[0];

            //Ensure logged in user has privileges to message target user
            List<PatientDoctor> pdList = await _patientDoctorService.GetPDByUserId(UserId);
            PatientDoctor verifiedPD = pdList.FirstOrDefault(pd =>
            (pd.PatientUserId == UserId && pd.DoctorUserId == receiverId) ||
            (pd.DoctorUserId == UserId && pd.PatientUserId == receiverId));

            if (verifiedPD == null) return Unauthorized(new { message = "User not authorized to send message to that account" });


            Message newMessage = await _messageservice.NewMessage(dto, senderId, receiverId);

            if (newMessage == null) return BadRequest(new { message = "An error occurred while creating new message" });

            return Ok(new { message = "Message successfully created" });
        }



        /*
            For loop uses PatientDoctor list above to find a matching connection based on the sender and receiver IDs. 
            This section of the method maps the patient and doctor names to the sender/receiver variables in the message dto.
            It also ensures the names remain current in the long term since names are retrieved directly from patient and doctor data
        */
        private List<MessageDto> MapMessagesToDto(List<PatientDoctor> pdList, List<Message> messages)
        {
            List<MessageDto> dtoList = new List<MessageDto>();

            foreach (Message m in messages)
            {
                PatientDoctor pdMatch = pdList.FirstOrDefault(pd =>
                (pd.PatientUserId == m.SenderId && pd.DoctorUserId == m.ReceiverId) ||
                (pd.PatientUserId == m.ReceiverId && pd.DoctorUserId == m.SenderId));

                if (pdMatch != null)
                {
                    string senderName = pdMatch.PatientUserId == m.SenderId ? pdMatch.Patient.GetPatientName() : pdMatch.Doctor.GetDoctorName();
                    string receiverName = pdMatch.PatientUserId == m.ReceiverId ? pdMatch.Patient.GetPatientName() : pdMatch.Doctor.GetDoctorName();

                    MessageDto dto = m.ToMessageDto(senderName, receiverName);
                    dtoList.Add(dto);
                }
            }

            return dtoList;
        }
    }
}
