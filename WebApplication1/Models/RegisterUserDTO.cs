using System.ComponentModel.DataAnnotations;

namespace Eindopdrachtcnd2.Models
{
    public class RegisterUserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
