using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
