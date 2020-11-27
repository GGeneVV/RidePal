using System.ComponentModel.DataAnnotations;

namespace RidePal.Web.Models
{
    public class RegisterVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username field is required")]
        [MinLength(5, ErrorMessage = "Username cannot be less than 5")]
        [MaxLength(20, ErrorMessage = "Username cannot be more than 20")]
        //[Remote("IsAlreadySigned", "Account", HttpMethod = "POST", ErrorMessage = "UserName already exists.")]
        public string UserName { get; set; }

        [MinLength(1, ErrorMessage = "FirstName cannot be less than 1")]
        public string FirstName { get; set; }

        [MinLength(1, ErrorMessage = "LastName cannot be less than 1")]
        public string LastName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password field is required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Password cannot be less than 4")]
        [MaxLength(100, ErrorMessage = "Password cannot be more than 100")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required(ErrorMessage = "E-mail required")]
        public string Email { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Upload Image")]
        public string ImageURL { get; set; }
    }
}
