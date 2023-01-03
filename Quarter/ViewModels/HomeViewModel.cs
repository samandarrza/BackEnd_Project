using Quarter.Models;

namespace Quarter.ViewModels
{
    public class HomeViewModel
    {
        public List<Aminity> Aminities { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Broker> Brokers { get; set; }
        public List<Category> Categories { get; set; }
        public List<City> Cities { get; set; }
        public List<House> Houses { get; set; }
        public List<Setting> Settings { get; set; }
        public List<About> Abouts { get; set; }
        public List<Feature> Features { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
