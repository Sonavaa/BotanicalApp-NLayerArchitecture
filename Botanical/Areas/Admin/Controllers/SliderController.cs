using AutoMapper;
using Business.DTOs.Slider;
using Business.IServices;
using Data.Context;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SliderController : Controller
    {
        private readonly ISliderService _SliderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SliderController(ISliderService SliderService, AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _SliderService = SliderService;
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _SliderService.GetAllSliderAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult AddSlider()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSlider(CreateSliderDTO addSliderDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(addSliderDTO);
            }


            try
            {
                await _SliderService.AddSlider(addSliderDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addSliderDTO);
            }
            //catch (Exception ex)
            //{
            //    // General exception (file upload, server errors, etc.)
            //    ModelState.AddModelError(string.Empty, ex.Message);
            //    return View(addSliderDTO);
            //}
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSlider(Guid id)
        {
            try
            {
                await _SliderService.DeleteSlider(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreSlider(Guid id)
        {
            try
            {
                await _SliderService.RestoreSlider(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSlider(Guid Id)
        {
            try
            {
                var Slider = await _SliderService.GetSliderById(Id);

                if (Slider == null)
                {
                    return NotFound("Slider not found.");
                }

                var updateSliderDTO = new CreateSliderDTO
                {
                    Id = Slider.Id,
                    Description = Slider.Description,
                    Title = Slider.Title,
                    SubTitle = Slider.SubTitle,
                    ImagePath = Slider.ImagePath,
                };

                return View(updateSliderDTO);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                return View("EditSlider", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSliderAsync(Guid id, CreateSliderDTO updateSliderDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(updateSliderDTO);
            }


            await _SliderService.EditSlider(id, updateSliderDTO);
            TempData["Success"] = "Slider updated successfully!";
            return RedirectToAction("Index");

        }

    }
}
