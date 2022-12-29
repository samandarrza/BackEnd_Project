using Quarter.Areas.Manage.ViewModels;
using Quarter.Models;

namespace Quarter.ViewModels
{
    public class ShopViewModel
    {
        public PaginatedList<House>? Houses { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Aminity>? Aminities { get; set; }
        public List<Broker>? Brokers { get; set; }
        public List<City>? Cities { get; set; }

        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
