using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
