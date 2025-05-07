using AutoMapper;
using Business.DTOs.Settings;
using Business.IServices;
using Data.Context;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _SettingService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SettingsController(ISettingsService SettingService, AppDbContext context, IMapper mapper)
        {
            _SettingService = SettingService;
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var settings = await _SettingService.GetAllSettings();
            return View(settings);
        }

        [HttpGet]
        public IActionResult AddSetting()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSetting(CreateSettingsDTO addSettingDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(addSettingDTO);
            }


            try
            {
                await _SettingService.AddSetting(addSettingDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addSettingDTO);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSetting(Guid id)
        {
            try
            {
                await _SettingService.DeleteSetting(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreSetting(Guid id)
        {
            try
            {
                await _SettingService.RestoreSetting(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSetting(Guid Id)
        {
            try
            {
                var Setting = await _SettingService.GetSettingByKey(Id);

                if (Setting == null)
                {
                    return NotFound("Setting not found.");
                }

                var updateSettingDTO = new CreateSettingsDTO
                {
                    Id = Setting.Id,
                    Key = Setting.Key,
                    Value = Setting.Value,
                    Title = Setting.Title,
                    Description = Setting.Description,
                    ImgPath = Setting.ImgPath,
                };

                return View(updateSettingDTO);
            }
            catch (Exception ex)
            {
                return View("EditSetting", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSettingAsync(Guid id, CreateSettingsDTO updateSettingDTO)
        {
            if (!ModelState.IsValid)
            {

                return View(updateSettingDTO);
            }


            await _SettingService.EditSetting(id, updateSettingDTO);
            TempData["Success"] = "Setting updated successfully!";
            return RedirectToAction("Index");

        }

    }
}
