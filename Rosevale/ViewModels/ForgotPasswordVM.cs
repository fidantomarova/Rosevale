using System.ComponentModel.DataAnnotations;

namespace Rosevale.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
