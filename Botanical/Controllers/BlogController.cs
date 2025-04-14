using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
