using System.ComponentModel.DataAnnotations.Schema;

namespace Rosevale.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Image { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
