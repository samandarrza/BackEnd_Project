using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
               .Include(x => x.Category).Include(x=>x.Broker)
               .Include(x => x.HouseImages)
               .Include(x => x.Comments).ThenInclude(x => x.AppUser)
               .Include(x => x.HouseAmenities).ThenInclude(x => x.Amenity)
               .FirstOrDefaultAsync(x => x.Id == id);

            HouseDetailViewModel detailVM = new HouseDetailViewModel
            {
                House = house,
                CommentVM = new CommentCreateViewModel { HouseId = house.Id},
                
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


    }
}
