using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
