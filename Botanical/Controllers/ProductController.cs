using Business.DTOs.Products;
using Business.IServices;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServiceForPresentation _productServiceForPresentation;

        public ProductController(IProductServiceForPresentation productServiceForPresentation)
        {
            _productServiceForPresentation = productServiceForPresentation;       
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await _productServiceForPresentation.ProductDetail(id);
            if (product == null)
                return NotFound();

            return View(product); 
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList(Guid id)
        {
            await _productServiceForPresentation.AddToWishList(id);
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishList(Guid id)
        {
            await _productServiceForPresentation.RemoveFromWishList(id);
            return RedirectToAction("Detail", new { id });
        }

        public async Task<IActionResult> Search(string search)
        {
            var products = await _productServiceForPresentation.Search(search);

            return PartialView("_SearchPartial", products ?? new List<GetProductDTO>());
        }
    }
}
