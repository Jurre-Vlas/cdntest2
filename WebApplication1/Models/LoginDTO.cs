using System.ComponentModel.DataAnnotations;

namespace Eindopdrachtcnd2.Models
{
    public class LoginDTO
    {
        [Required]
       public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
