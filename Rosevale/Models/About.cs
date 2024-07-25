using System.ComponentModel.DataAnnotations.Schema;

namespace Rosevale.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        public string Img3 { get; set; }
        public string Img4 { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
