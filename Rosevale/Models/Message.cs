using System.ComponentModel.DataAnnotations;

namespace Rosevale.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Message is required")]
        public string MessageDescription { get; set; }
        public bool IsReplied { get; set; }
    }
}
