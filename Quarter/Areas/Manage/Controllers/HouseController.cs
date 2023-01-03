using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Quarter.Areas.Manage.ViewModels;
using Quarter.DAL;
using Quarter.Helpers;
using Quarter.Models;
using System.Data;

namespace Quarter.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]

    public class HouseController : Controller
    {
        private readonly QuarterDbContext _context;
        private readonly IWebHostEnvironment _env;
        public HouseController(QuarterDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Houses
            .Include(x => x.Category)
            .Include(x => x.HouseAmenities).ThenInclude(x=>x.Amenity)
            .Include(x=>x.HouseImages)
            .Include(x => x.City)
            .Include(x=>x.Broker);
            
            var model = PaginatedList<House>.Create(query, page, 20);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brokers = _context.Brokers.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Aminities = _context.Aminities.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(House house)
        {
            if (!_context.Categories.Any(x => x.Id == house.CategoryId))
                ModelState.AddModelError("CategoryId", "Category not found");

            if (!_context.Brokers.Any(x => x.Id == house.BrokerId))
                ModelState.AddModelError("BrokerId", "Broker not found");

            if (!_context.Cities.Any(x => x.Id == house.CityId))
                ModelState.AddModelError("CityId", "City not found");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Brokers = _context.Brokers.ToList();
                ViewBag.Cities = _context.Cities.ToList();
                ViewBag.Aminities = _context.Aminities.ToList();
                return View();
            }

            house.HouseImages = new List<HouseImage>();

            if (house.PosterFile == null)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Brokers = _context.Brokers.ToList();
                ViewBag.Cities = _context.Cities.ToList();
                ViewBag.Aminities = _context.Aminities.ToList();
                ModelState.AddModelError("PosterFile", "Required");
                return View();
            }
            HouseImage poster = new HouseImage
            {
                Name = FileManager.Save(house.PosterFile, _env.WebRootPath, "main/uploads/houses"),
                PosterStatus = true,
            };
            house.HouseImages.Add(poster);

            if (house.ImageFiles == null)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Brokers = _context.Brokers.ToList();
                ViewBag.Cities = _context.Cities.ToList();
                ViewBag.Aminities = _context.Aminities.ToList();
                ModelState.AddModelError("ImageFiles", "Required");
                return View();
            }

            foreach (var imgFile in house.ImageFiles)
            {
                HouseImage houseImage = new HouseImage
                {
                    Name = FileManager.Save(imgFile, _env.WebRootPath, "main/uploads/houses")
                };
                house.HouseImages.Add(houseImage);
            }

            house.HouseAmenities = new List<HouseAmenity>();

            if (house.AminityIds == null || house.AminityIds.Count !=4)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Brokers = _context.Brokers.ToList();
                ViewBag.Cities = _context.Cities.ToList();
                ViewBag.Aminities = _context.Aminities.ToList();
                ModelState.AddModelError("AminityIds", "Amenity 4 pieces should be selected");
                return View();
            }


            foreach (var aminityId in house.AminityIds)
            {
                if (!_context.Aminities.Any(x => x.Id == aminityId))
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    ViewBag.Brokers = _context.Brokers.ToList();
                    ViewBag.Cities = _context.Cities.ToList();
                    ViewBag.Aminities = _context.Aminities.ToList();

                    ModelState.AddModelError("AminityIds", "Amenity not found");
                    return View();
                }

                HouseAmenity houseAminity = new HouseAmenity
                {
                    AmenityId = aminityId
                };
                house.HouseAmenities.Add(houseAminity);
            }
            house.Status = true;
         
            _context.Houses.Add(house);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            House house = _context.Houses.Include(x => x.HouseAmenities).Include(x => x.HouseImages).FirstOrDefault(x => x.Id == id);

            if (house == null)
                return RedirectToAction("error", "dashboard");
            
            
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brokers = _context.Brokers.ToList();
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Aminities = _context.Aminities.ToList();

            house.AminityIds = house.HouseAmenities.Select(x => x.AmenityId).ToList();
            return View(house);
        }
        [HttpPost]
        public IActionResult Edit(House house)
        {
            House existHouse = _context.Houses.Include(x => x.HouseAmenities).Include(x => x.HouseImages).FirstOrDefault(x => x.Id == house.Id);

            if (existHouse == null)
                return RedirectToAction("error", "dashboard");

            if (existHouse.CategoryId != house.CategoryId && !_context.Categories.Any(x => x.Id == house.CategoryId))
                ModelState.AddModelError("CategoryId", "Category not found");

            if (existHouse.BrokerId != house.BrokerId && !_context.Brokers.Any(x => x.Id == house.BrokerId))
                ModelState.AddModelError("BrokerId", "Broker not found");

            if (existHouse.CityId != house.CityId && !_context.Cities.Any(x => x.Id == house.CityId))
                ModelState.AddModelError("CityId", "City not found");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Brokers = _context.Brokers.ToList();
                ViewBag.Cities = _context.Cities.ToList();
                ViewBag.Aminities = _context.Aminities.ToList();

                foreach (var aminity in existHouse.HouseAmenities)
                {
                    house.AminityIds.Add(aminity.AmenityId);
                }

                return View(existHouse);
            }

            if (house.PosterFile != null)
            {
                var poster = existHouse.HouseImages.FirstOrDefault(x => x.PosterStatus == true);
                var newPosterName = FileManager.Save(house.PosterFile, _env.WebRootPath, "main/uploads/houses");
                FileManager.Delete(_env.WebRootPath, "main/uploads/houses", poster.Name);
                poster.Name = newPosterName;
            }

            var removedFiles = existHouse.HouseImages.FindAll(x => x.PosterStatus == false && !house.HouseImageIds.Contains(x.Id));
            foreach (var item in removedFiles)
            {
                FileManager.Delete(_env.WebRootPath, "main/uploads/houses", item.Name);
            }

            existHouse.HouseImages.RemoveAll(x => removedFiles.Contains(x));

            if (house.ImageFiles != null)
            {
                foreach (var imgFile in house.ImageFiles)
                {
                    HouseImage houseImage = new HouseImage
                    {
                        Name = FileManager.Save(imgFile, _env.WebRootPath, "main/uploads/houses"),
                    };
                    existHouse.HouseImages.Add(houseImage);
                }
            }

            existHouse.HouseAmenities.RemoveAll(x => !house.AminityIds.Contains(x.AmenityId));


            foreach (var aminityId in house.AminityIds.Where(x => !existHouse.HouseAmenities.Any(bt => bt.AmenityId == x)))
            {
                if (!_context.Aminities.Any(x => x.Id == aminityId))
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    ViewBag.Brokers = _context.Brokers.ToList();
                    ViewBag.Cities = _context.Cities.ToList();
                    ViewBag.Aminities = _context.Aminities.ToList();

                    foreach (var bt in existHouse.HouseAmenities)
                    {
                        house.AminityIds.Add(bt.AmenityId);
                    }

                    ModelState.AddModelError("AminityIds", "Aminity not found");
                    return View();
                }

                HouseAmenity houseAmenity = new HouseAmenity
                {
                    AmenityId = aminityId
                };
                existHouse.HouseAmenities.Add(houseAmenity);
            }

            existHouse.Name = house.Name;
            existHouse.Desc = house.Desc;
            existHouse.RoomsCount = house.RoomsCount;
            existHouse.BedRoomCount = house.BedRoomCount;
            existHouse.BathRoomCount = house.BathRoomCount;
            existHouse.BuiltYear = house.BuiltYear;
            existHouse.SquareFt = house.SquareFt;
            existHouse.CarParking = house.CarParking;
            existHouse.Status = house.Status;
            existHouse.SalePrice = house.SalePrice;
            existHouse.CostPrice = house.CostPrice;
            existHouse.DiscountPercent = house.DiscountPercent;
            existHouse.BrokerId = house.BrokerId;
            existHouse.CategoryId = house.CategoryId;
            existHouse.CityId = house.CityId;

            _context.SaveChanges();
            return RedirectToAction("index");

        }

    }
}
