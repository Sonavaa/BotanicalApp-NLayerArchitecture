using AutoMapper;
using Business.DTOs.Products;
using Business.Services;
using Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
		public ProductController(IProductService productService, AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _productService = productService;
			_context = context;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(CreateProductDTO addProductDTO)
        {

            if (!ModelState.IsValid)
            {
				return View();
            }


            try
            {
                await _productService.AddProduct(addProductDTO);

                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addProductDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreProduct(Guid id)
        {
            try
            {
                await _productService.RestoreProduct(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> EditProduct(Guid Id)
        //{
        //    try
        //    {

        //        var product = _productService.GetProductById(Id);

        //        if (product == null)
        //        {
        //            return NotFound("Product not found.");
        //        }

        //        var updateProductDTO = new CreateProductDTO
        //        {
        //            Name = updateProductDTO.Name,                    
        //            Price = updateProductDTO.Price,
        //            DiscountedPrice = updateProductDTO.DiscountedPrice,


        //        };

        //        return View(updateProductDTO);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex.Message); 
        //    }
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductAsync(Guid id,  CreateProductDTO updateProductDTO)
        {
            if (!ModelState.IsValid)
            {

				return View(updateProductDTO);
            }

           
                await _productService.EditProduct(id, updateProductDTO);
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction("Index");  
            
        }

    }
}
