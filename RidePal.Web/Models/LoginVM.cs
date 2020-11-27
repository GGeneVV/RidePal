using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Are you really trying to login without entering username?")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password:)")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password cannot be less than 4")]
        [MaxLength(100, ErrorMessage = "Password cannot be more than 100")]
        public string Password { get; set; }
        [Display(Name = "Stay logged in when browser is closed")]
        public bool RememberMe { get; set; }
    }
}
