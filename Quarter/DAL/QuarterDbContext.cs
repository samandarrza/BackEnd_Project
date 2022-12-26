using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quarter.Models;

namespace Quarter.DAL
{
    public class QuarterDbContext: IdentityDbContext
    {
        public QuarterDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Aminity> Aminities { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<HouseAmenity> HousesAmenities { get; set; }
        public DbSet<HouseImage> HouseImages { get; set; }
    }
}
