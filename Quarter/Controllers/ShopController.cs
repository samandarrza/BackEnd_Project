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
        public IActionResult Index(int page = 1, List<int>? cityId = null, List<int>? aminityId = null, List<int>? categoryId = null, List<int>? brokerId = null,
            decimal? minPrice = null, decimal? maxPrice = null, string? sort = "", string? search = "")
        {
            ViewBag.SelectedCityIds = cityId;
            ViewBag.SelectedAmenityIds = aminityId;
            ViewBag.SelectedCategoryIds = categoryId;
            ViewBag.SelectedBrokerIds = brokerId;
            ViewBag.SelectedSearch = search;



            var houses = _context.Houses.Include(x => x.HouseImages).Include(x => x.HouseAmenities).ThenInclude(x=>x.Amenity)
                .Include(x=> x.City).Include(x => x.Broker).Include(x => x.Category).AsQueryable();

            if (cityId != null && cityId.Count > 0)
                houses = houses.Where(x => cityId.Contains(x.CityId));

            if (categoryId != null && categoryId.Count > 0)
                houses = houses.Where(x => categoryId.Contains(x.CategoryId));

            if (brokerId != null && brokerId.Count > 0)
                houses = houses.Where(x => brokerId.Contains(x.BrokerId));

            if (aminityId != null && aminityId.Count > 0)
                houses = houses.Where(x => x.HouseAmenities.Any(ha=> aminityId.Contains(ha.AmenityId)));
            
            if (search != null)
                houses = houses.Where(x => x.Name.Contains(search));

            if (minPrice != null && maxPrice != null)
            {
                houses = houses.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);

            }

            switch (sort)
            {
                case "AToZ":
                    houses = houses.OrderBy(x => x.Name);
                    break;
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
