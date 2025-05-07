using AutoMapper;
using Business.DTOs.Category;
using Business.IServices;
using Business.Services;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Botanical.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
		public CategoryController(ICategoryService CategoryService, AppDbContext context, IMapper mapper)
		{
            _CategoryService = CategoryService;
			_context = context;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
        {
            var categories = await _CategoryService.GetAllCategoryAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CreateCategoryDTO addCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(addCategoryDTO);
            }


            try
            {
                await _CategoryService.AddCategory(addCategoryDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addCategoryDTO);
            }
            //catch (Exception ex)
            //{
            //    // General exception (file upload, server errors, etc.)
            //    ModelState.AddModelError(string.Empty, ex.Message);
            //    return View(addCategoryDTO);
            //}
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _CategoryService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreCategory(Guid id)
        {
            try
            {
                await _CategoryService.RestoreCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(Guid Id)
        {
            try
            {
                var category = await _CategoryService.GetCategoryById(Id);

                if (category == null)
                {
                    return NotFound("Category not found.");
                }

                var updateCategoryDTO = new CreateCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImgPath = category.ImgPath,
                };

                return View(updateCategoryDTO);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                return View("EditCategory", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategoryAsync(Guid id,  CreateCategoryDTO updateCategoryDTO)
        {
            if (!ModelState.IsValid)
            {

				return View(updateCategoryDTO);
            }

           
                await _CategoryService.EditCategory(id, updateCategoryDTO);
                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction("Index");  
            
        }

    }
}
