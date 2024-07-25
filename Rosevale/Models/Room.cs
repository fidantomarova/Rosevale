using System.ComponentModel.DataAnnotations.Schema;

namespace Rosevale.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Price { get; set; }
        public int Rate { get; set; }
        public int BedCount { get; set; }
        public int BathCount { get; set; }
        public bool IsWifi { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public List<Reservation> Reservation { get; set; }
    }
}
