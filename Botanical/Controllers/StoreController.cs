using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
