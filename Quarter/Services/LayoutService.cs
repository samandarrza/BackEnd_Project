using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quarter.DAL;
using Quarter.Models;
using Quarter.ViewModels;
using System.Security.Claims;

namespace Quarter.Services
{
    public class LayoutService
    {
        private readonly QuarterDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public LayoutService(QuarterDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }

        public BasketViewModel GetBasket()
        {
            BasketViewModel basket = new BasketViewModel();

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated && _contextAccessor.HttpContext.User.IsInRole("Member"))
            {
                string userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var model = _context.BasketItems.Include(x => x.House).ThenInclude(x => x.HouseImages).Where(x => x.AppUserId == userId).ToList();
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
                var basketStr = _contextAccessor.HttpContext.Request.Cookies["basket"];
                List<BasketItemCookieViewModel> basketItemsCookie = new List<BasketItemCookieViewModel>();
                if (basketStr != null)
                {
                    basketItemsCookie = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
                }

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

            return basket;
        }


    }
}
