using Botanical.ViewModels.HomeViewModels;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Botanical.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                //Products = await _context.Products.Include(p=>p.Images).Where(p => p.isDeleted == false).ToListAsync()

            };
            return View(homeVM);
        }
    }
}
