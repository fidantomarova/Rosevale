using System.ComponentModel.DataAnnotations.Schema;

namespace Rosevale.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public string ClientName { get; set; }
        public string Profession { get; set; }
        public string Image { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
