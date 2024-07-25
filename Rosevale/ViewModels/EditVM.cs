using System.ComponentModel.DataAnnotations;

namespace Rosevale.ViewModels
{
    public class EditVM
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
