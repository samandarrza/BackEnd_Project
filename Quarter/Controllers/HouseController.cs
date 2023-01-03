using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quarter.DAL;
using Quarter.Models;
using Quarter.ViewModels;
using System.Data;

namespace Quarter.Controllers
{
    public class HouseController : Controller
    {
        private readonly QuarterDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HouseController(QuarterDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {

            House house = await _context.Houses
               .Include(x => x.City)
               .Include(x => x.Category).Include(x => x.Broker)
               .Include(x => x.HouseImages)
               .Include(x => x.Comments).ThenInclude(x => x.AppUser)
               .Include(x => x.HouseAmenities).ThenInclude(x => x.Amenity)
               .FirstOrDefaultAsync(x => x.Id == id);

            HouseDetailViewModel detailVM = new HouseDetailViewModel
            {
                House = house,
                CommentVM = new CommentCreateViewModel { HouseId = house.Id },

                RelatedHouses = _context.Houses.Include(x => x.City)
               .Include(x => x.Category).Include(x => x.Broker)
               .Include(x => x.HouseImages)
               .Include(x => x.Comments).ThenInclude(x => x.AppUser)
               .Include(x => x.HouseAmenities).ThenInclude(x => x.Amenity).Where(x => x.City == house.City || x.Broker == house.Broker)
                .Take(4).ToList(),
            };

            if (house == null)
                return NotFound();

            return View(detailVM);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Comment(CommentCreateViewModel commentVM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            House house = await _context.Houses
              .Include(x => x.City)
               .Include(x => x.Category).Include(x => x.Broker)
              .Include(x => x.HouseImages)
              .Include(x => x.Comments).ThenInclude(x => x.AppUser)
              .Include(x => x.HouseAmenities).ThenInclude(x => x.Amenity)
              .FirstOrDefaultAsync(x => x.Id == commentVM.HouseId);

            if (house == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                HouseDetailViewModel detailVM = new HouseDetailViewModel
                {
                    House = house,
                    RelatedHouses = _context.Houses.Include(x => x.City)
               .Include(x => x.Category).Include(x => x.Broker)
               .Include(x => x.HouseImages)
               .Include(x => x.Comments).ThenInclude(x => x.AppUser)
               .Include(x => x.HouseAmenities).ThenInclude(x => x.Amenity).Where(x => x.City == house.City || x.Broker == house.Broker)
                .Take(4).ToList(),
                    CommentVM = commentVM
                };

                return View("detail", detailVM);
            }

            Comment newComment = new Comment
            {
                Text = commentVM.Text,
                AppUserId = user.Id,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            house.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("detail", new { id = house.Id });
        }

        public async Task<IActionResult> AddToBasket(int houseId)
        {
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            BasketViewModel basket = new BasketViewModel();

            if (!_context.Houses.Any(x => x.Id == houseId && x.Status))
            {
                return NotFound();
            }

            if (user != null)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.HouseId == houseId && x.AppUserId == user.Id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        HouseId = houseId,
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    _context.BasketItems.Remove(basketItem);
                }

                _context.SaveChanges();

                var model = _context.BasketItems.Include(x => x.House).ThenInclude(x => x.HouseImages)
                    .Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in model)
                {
                    BasketItemViewModel itemVM = new BasketItemViewModel
                    {
                        House = item.House,
                        Id = item.Id
                    };
                    basket.Items.Add(itemVM);
                    basket.totalPrice += (item.House.SalePrice * (100 - item.House.DiscountPercent) / 100);
                }
            }
            else
            {
                var basketStr = HttpContext.Request.Cookies["basket"];
                List<BasketItemCookieViewModel> basketItemsCookie = null;

                if (basketStr == null)
                {
                    basketItemsCookie = new List<BasketItemCookieViewModel>();
                }
                else
                {
                    basketItemsCookie = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
                }

                BasketItemCookieViewModel basketCookieItem = basketItemsCookie.FirstOrDefault(x => x.HouseId == houseId);

                if (basketCookieItem == null)
                {
                    basketCookieItem = new BasketItemCookieViewModel
                    {
                        HouseId = houseId,
                    };

                    basketItemsCookie.Add(basketCookieItem);
                }
                else
                {
                    basketItemsCookie.Remove(basketCookieItem);
                }

                var jsonStr = JsonConvert.SerializeObject(basketItemsCookie);
                HttpContext.Response.Cookies.Append("basket", jsonStr);

                foreach (var item in basketItemsCookie)
                {
                    House house = _context.Houses.Include(x => x.HouseImages).FirstOrDefault(x => x.Id == item.HouseId);
                    BasketItemViewModel itemVM = new BasketItemViewModel
                    {
                        House = house,
                        Id = 0
                    };
                    basket.Items.Add(itemVM);
                    basket.totalPrice += (itemVM.House.SalePrice * (100 - itemVM.House.DiscountPercent) / 100);
                }
            }
            return PartialView("_BasketPartial", basket);
        }
    }
}
