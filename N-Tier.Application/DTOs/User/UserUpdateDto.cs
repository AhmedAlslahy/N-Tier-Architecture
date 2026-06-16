using Microsoft.AspNetCore.Http;

namespace N_Tier.Application.DTOs.User;

public class UserUpdateDto
{
    public required string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileDescription { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? Image { get; set; }
    public string? PublicLink { get; set; }
}