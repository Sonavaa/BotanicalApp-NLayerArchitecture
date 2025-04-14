using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
