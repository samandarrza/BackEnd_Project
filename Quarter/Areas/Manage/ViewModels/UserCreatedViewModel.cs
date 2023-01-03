using Microsoft.AspNetCore.Identity;
using Quarter.Models;

namespace Quarter.Areas.Manage.ViewModels
{
    public class UserCreatedViewModel
    {
        public AppUser AppUser { get; set; }
        public IdentityRole Role { get; set; }
    }
}
