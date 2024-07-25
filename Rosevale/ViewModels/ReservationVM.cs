using Rosevale.Models;

namespace Rosevale.ViewModels
{
    public class ReservationVM
    {
        public About About { get; set; }
        public Reservation Reservation { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
