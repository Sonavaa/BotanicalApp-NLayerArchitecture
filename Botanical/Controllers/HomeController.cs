using AutoMapper;
using Botanical.ViewModels.HomeViewModels;
using Business.DTOs.Products;
using Business.DTOs.Slider;
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

                Sliders = await _mapper.ProjectTo<GetSliderDTO>(
             _context.Sliders.Where(s => !s.IsDeleted)
         ).ToListAsync()
            };
            return View(homeVM);
        }

    }
}
