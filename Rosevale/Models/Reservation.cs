using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rosevale.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Check in date is required")]
        public DateTime CheckIn { get; set; }
        [Required(ErrorMessage = "Check out date is required")]
        public DateTime CheckOut { get; set; }
        public Reservation()
        {

            CheckIn = DateTime.UtcNow.AddHours(4);
            CheckOut = DateTime.UtcNow.AddHours(4);
        }
        [Required(ErrorMessage = "Adult count is required")]
        public int Adults { get; set; }
        [Required(ErrorMessage = "Child count is required")]
        public int Children { get; set; }
        public string SpecialRequest { get; set; }
        public bool IsDeactive { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
    }
}
