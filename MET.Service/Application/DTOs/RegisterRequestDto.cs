using System.ComponentModel.DataAnnotations;

namespace MET.Service.Application.DTOs
{
    public class RegisterRequestDto
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        [Required]
        [EmailAddress] 
        public string? Email { get; set; }
    }
}