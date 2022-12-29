using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.Areas.Manage.ViewModels;
using Quarter.DAL;
using Quarter.Models;
using Quarter.ViewModels;

namespace Quarter.Controllers
{
    public class ShopController : Controller
    {
        private readonly QuarterDbContext _context;

        public ShopController(QuarterDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, List<int>? CityIds = null, List<int>? AmenityIds = null, List<int>? CategotyIds = null, List<int>? BrokerIds = null,
            decimal? minPrice = null, decimal? maxPrice = null, string? sort = "AToZ")
        {
            ViewBag.SelectedCityIds = CityIds;
            ViewBag.SelectedAmenityIds = AmenityIds;
            ViewBag.SelectedCategoryIds = CategotyIds;
            ViewBag.SelectedBrokerIds = BrokerIds;


            var houses = _context.Houses.Include(x => x.HouseImages).Include(x => x.HouseAmenities).ThenInclude(x=>x.Amenity)
                .Include(x=> x.City).Include(x => x.Broker).Include(x => x.Category).AsQueryable();

            if (CityIds != null && CityIds.Count > 0)
                houses = houses.Where(x => CityIds.Contains(x.CityId));

            if (CategotyIds != null && CategotyIds.Count > 0)
                houses = houses.Where(x => CategotyIds.Contains(x.CategoryId));

            if (BrokerIds != null && BrokerIds.Count > 0)
                houses = houses.Where(x => BrokerIds.Contains(x.BrokerId));

            if (AmenityIds != null && AmenityIds.Count > 0)
                houses = houses.Where(x => x.HouseAmenities.Any(ha=>AmenityIds.Contains(ha.AmenityId)));


            if (minPrice != null && maxPrice != null)
            {
                houses = houses.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);
            }

            switch (sort)
            {
                case "ZToA":
                    houses = houses.OrderByDescending(x => x.Name);
                    break;
                case "LowToHigh":
                    houses = houses.OrderBy(x => x.SalePrice);
                    break;
                case "HighToLow":
                    houses = houses.OrderByDescending(x => x.SalePrice);
                    break;
                default:
                    houses = houses.OrderBy(x => x.Name);
                    break;
            }

            ShopViewModel model = new ShopViewModel
            {
                Houses = PaginatedList<House>.Create(houses, page, 6),
                Cities = _context.Cities.Where(x => x.Houses.Count > 0).ToList(),
                Categories = _context.Categories.Include(x => x.Houses).Where(x => x.Houses.Count > 0).ToList(),
                Aminities = _context.Aminities.ToList(),
                Brokers = _context.Brokers.Where(x => x.Houses.Count > 0).ToList(),
                MinPrice = _context.Houses.Min(x => x.SalePrice),
                MaxPrice = _context.Houses.Max(x => x.SalePrice)
            };

            ViewBag.SelectedMinPrice = minPrice ?? model.MinPrice;
            ViewBag.SelectedMaxPrice = maxPrice ?? model.MaxPrice;


            return View(model);
        }
    }
}
