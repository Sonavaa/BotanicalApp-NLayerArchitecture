using Business;
using Business.Profiles;
using Business.Validators.Category;
using Business.Validators.Contact;
using Business.Validators.Product;
using Business.Validators.Settings;
using Business.Validators.Slider;
using Business.Validators.User;
using Core.Entities.Identity;
using Data.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    //options.SignIn.RequireConfirmedAccount = false;
    //options.User.RequireUniqueEmail = false;

    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;

    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
    options.Lockout.MaxFailedAccessAttempts = 300;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateProductDTOValidator>())
      .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCategoryDTOValidator>())
       .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateSliderDTOValidator>())
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateSettingsDTOValidator>())
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginDTOValidator>())
             .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterDTOValidator>())
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateContactDTOValidator>());


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapping));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();

app.UseStaticFiles(); 

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboards}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
