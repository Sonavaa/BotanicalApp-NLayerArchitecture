using AutoMapper;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.IServices;
using Business.Services;
using Core.Entities;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Botanical.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
		public ProductController(IProductService productService, ICategoryService categoryService, AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _productService = productService;
            _categoryService = categoryService;
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
        public IActionResult AddProduct()
        {
            var categories = _context.Categories.Where(c=>!c.IsDeleted).ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(CreateProductDTO addProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(addProductDTO);
            }

            try
            {
                await _productService.AddProduct(addProductDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addProductDTO);
            }
            //catch (Exception ex)
            //{
            //    // General exception (file upload, server errors, etc.)
            //    ModelState.AddModelError(string.Empty, ex.Message);
            //    return View(addCategoryDTO);
            //}
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

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid Id)
        {
            try
            {
                var product = await _productService.GetProductById(Id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var categories = await _categoryService.GetAllCategoryAsync(); 
                if (categories == null || !categories.Any())
                {
                    return View("Error","No categories found." );
                }

                var updateProductDTO = new CreateProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImagePath = product.ImagePath,
                    Price = product.Price,
                    DiscountedPrice = product.DiscountedPrice,
                    Quantity = product.Quantity,
                    ProductCode = product.ProductCode,
                    CategoryId = product.CategoryId 
                };

                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                return View(updateProductDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
