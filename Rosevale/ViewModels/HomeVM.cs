using Rosevale.Models;

namespace Rosevale.ViewModels
{
    public class HomeVM
    {
        public List<Carousel> Carousels { get; set; }
        public About About { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Service> Services { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<SocialMedia> SocialMedias { get; set; }
        public List<Staff> Staffs { get; set; }
    }
}
