namespace Clips.Application.DTOs.Account
{
    public class UserDto
    {
        public string? id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string JWT { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
