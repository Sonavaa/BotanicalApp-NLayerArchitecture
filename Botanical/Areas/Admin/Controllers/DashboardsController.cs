using Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Areas.Admin.Controllers;

[Area(nameof(Admin))]
//[Authorize(Policy = "AdminOnly")]
public class DashboardsController : Controller
{
    private readonly AppDbContext _context;

    public DashboardsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}
