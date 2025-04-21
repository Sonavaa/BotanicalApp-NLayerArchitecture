using Business;
using Business.Profiles;
using Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container
builder.Services.AddControllersWithViews();

// 2. Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// 3. Add custom services
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices(builder.Configuration);

// 4. Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapping));

var app = builder.Build();

// 5. Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 6. Static files
app.UseStaticFiles(); // ?? Bu x?tt ??rh? al?nm??d?, onu açd?m

app.UseRouting();
app.UseAuthorization();

// 7. Area routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboards}/{action=Index}/{id?}");

// 8. Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 9. ?g?r .WithStaticAssets() xüsusi bir extension method-dursa, burada saxlaya bil?rsiniz.
// ?ks halda onu silin. ?g?r bilmirsinizs?, onu da izah ed? bil?r?m.
app.Run();
