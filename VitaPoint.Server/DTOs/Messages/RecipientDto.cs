/*
    
    This DTO is used to transfer data of possible receipients for all users when creating a new message
 
 */

namespace VitaPoint.Server.DTOs.Messages
{
    public class RecipientDto
    {
        public string? SysId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; } = "";
    }
}
