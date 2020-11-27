using System.ComponentModel.DataAnnotations;

namespace RidePal.Services.LoginCredentialsModel
{
    public class LoginCredentialsModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}