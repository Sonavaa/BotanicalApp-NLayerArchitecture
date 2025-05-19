using AutoMapper;
using Botanical.ViewModels.HomeViewModels;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Business.DTOs.Subscriber;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Botanical.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public HomeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Products = await _mapper.ProjectTo<GetProductDTO>(
             _context.Products.Where(p => !p.IsDeleted)
         ).ToListAsync(),

                Categories = await _mapper.ProjectTo<GetCategoryDTO>(
             _context.Categories.Where(p => !p.IsDeleted)
         ).ToListAsync(),

                Sliders = await _mapper.ProjectTo<GetSliderDTO>(
             _context.Sliders.Where(s => !s.IsDeleted)
         ).ToListAsync(),
                Settings = await _mapper.ProjectTo<GetSettingsDTO>(
             _context.Settings.Where(s => !s.IsDeleted)
         ).ToListAsync(),
                Subscriber = await _mapper.ProjectTo<GetSubscriberDTO>(
    _context.Subscribers
).FirstOrDefaultAsync()
            };
            return View(homeVM);
        }

    }
}
