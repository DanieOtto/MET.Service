namespace MET.Service.Application.DTOs
{
    public class RegisterRequestDto
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public required string[] Roles { get; set; }
        public required string[] Scopes { get; set; }
    }
}