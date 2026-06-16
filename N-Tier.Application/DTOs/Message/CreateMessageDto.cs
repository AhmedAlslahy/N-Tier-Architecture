using Microsoft.AspNetCore.Http;

namespace N_Tier.Application.DTOs.Message
{
    public class CreateMessageDto
    {
        public string? Content { get; set; }
        public string? PhotoUrl { get; set; }
        public IFormFile? Photo { get; set; }
        public required string ReceiverId { get; set; }
    }
}