using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quarter.DAL;
using Quarter.Models;
using Quarter.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<QuarterDbContext>(opt =>
{
    opt.UseSqlServer("Server=DESKTOP-HO9CBPN\\SQLEXPRESS;Database=QuarterDb; Trusted_Connection=TRUE");
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddDefaultTokenProviders().AddEntityFrameworkStores<QuarterDbContext>();

builder.Services.AddScoped<LayoutService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/manage/account/login" + redirectPath.Query);
        }
        else
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/account/login" + redirectPath.Query);
        }

        return Task.CompletedTask;
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
       name: "areas",
       pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();