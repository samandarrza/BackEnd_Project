using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quarter.DAL;
using Quarter.Models;
using Quarter.ViewModels;
using System.Security.Claims;

namespace Quarter.Controllers
{
    //public class OrderController : BaseController
    //{
    //    private readonly QuarterDbContext _context;
    //    private readonly UserManager<AppUser> _userManager;

    //    public OrderController(QuarterDbContext context, UserManager<AppUser> userManager)
    //    {
    //        _context = context;
    //        _userManager = userManager;
    //    }
    //    public async Task<IActionResult> Checkout()
    //    {
    //        var model = await _getCheckoutVM();
    //        return View(model);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Checkout(Order order)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            var model = await _getCheckoutVM();
    //            model.Order = order;
    //            return View(model);
    //        }
    //        List<BasketItem> basketItems = new List<BasketItem>();
    //        if (User.Identity.IsAuthenticated)
    //        {
    //            basketItems = _context.BasketItems.Include(x => x.House).Where(x => x.AppUserId == UserId).ToList();
    //            order.AppUserId = UserId;
    //            _context.BasketItems.RemoveRange(basketItems);
    //        }
    //        else
    //        {
    //            basketItems = _mapBasketItems(_getCookieBasketItems());
    //            Response.Cookies.Delete("basket");
    //        }

    //        order.OrderItems = basketItems.Select(b => new OrderItem
    //        {
    //            HouseId = b.HouseId,
    //            Count = b.Count,
    //            DiscountPercent = b.Book.DiscountPercent,
    //            CostPrice = b.Book.CostPrice,
    //            SalePrice = b.Book.SalePrice,
    //            Name = b.Book.Name
    //        }).ToList();


    //        _context.Orders.Add(order);

    //        _context.SaveChanges();
    //        return RedirectToAction("index", "home");
    //    }


    //    private async Task<CheckoutViewModel> _getCheckoutVM()
    //    {
    //        List<BasketItem> basketItems = new List<BasketItem>();
    //        CheckoutViewModel model = new CheckoutViewModel();

    //        if (User.Identity.IsAuthenticated)
    //        {
    //            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //            AppUser user = await _userManager.FindByIdAsync(userId);

    //            model.Order = new Order
    //            {
    //                Fullname = user.FullName,
    //                Email = user.Email
    //            };

    //            basketItems = _context.BasketItems.Include(x => x.Book).Where(x => x.AppUserId == userId).ToList();
    //        }
    //        else
    //        {
    //            basketItems = _mapBasketItems(_getCookieBasketItems());
    //        }
    //        model.CheckoutItems = basketItems.Select(item => new CheckoutItemViewModel
    //        {
    //            Count = item.Count,
    //            Name = item.Book.Name,
    //            TotalPrice = (item.Book.SalePrice * (100 - item.Book.DiscountPercent) / 100) * item.Count,
    //        }).ToList();


    //        model.Total = model.CheckoutItems.Sum(x => x.TotalPrice);
    //        return model;
    //    }

    //    private List<BasketItemCookieViewModel> _getCookieBasketItems()
    //    {
    //        var basketStr = HttpContext.Request.Cookies["basket"];

    //        List<BasketItemCookieViewModel> basketCookieItems = new List<BasketItemCookieViewModel>();

    //        if (basketStr != null)
    //        {
    //            basketCookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);
    //        }
    //        return basketCookieItems;
    //    }

    //    private List<BasketItem> _mapBasketItems(List<BasketItemCookieViewModel> basketCookieItems)
    //    {
    //        List<BasketItem> basketItems = new List<BasketItem>();
    //        foreach (var item in basketCookieItems)
    //        {
    //            Book book = _context.Books.FirstOrDefault(x => x.Id == item.BookId && x.StockStatus);
    //            if (book == null) continue;

    //            BasketItem basketItem = new BasketItem
    //            {
    //                Count = item.Count,
    //                BookId = item.BookId,
    //                Book = book
    //            };
    //            basketItems.Add(basketItem);
    //        }
    //        return basketItems;
    //    }
    //}
}
