using System.ComponentModel.DataAnnotations.Schema;

namespace Rosevale.Models
{
    public class Carousel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
