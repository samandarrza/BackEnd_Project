

using Microsoft.EntityFrameworkCore;
using Quarter.DAL;
using Quarter.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuarterDbContext>(opt =>
{
    opt.UseSqlServer("Server=DESKTOP-HO9CBPN\\SQLEXPRESS;Database=QuarterDb; Trusted_Connection=TRUE");
});
builder.Services.AddScoped<LayoutService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
       name: "areas",
       pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();