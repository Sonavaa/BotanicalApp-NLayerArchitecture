using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
