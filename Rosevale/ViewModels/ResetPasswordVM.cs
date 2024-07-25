using System.ComponentModel.DataAnnotations;

namespace Rosevale.ViewModels
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Repeat Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string RepeatPassword { get; set; }
        public string Code { get; set; }
    }
}
