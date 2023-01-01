using Quarter.Models;

namespace Quarter.ViewModels
{
    public class HouseDetailViewModel
    {
        public House House { get; set; }
        public List<House> RelatedHouses { get; set; }
        public CommentCreateViewModel CommentVM { get; set; }
    }
}
