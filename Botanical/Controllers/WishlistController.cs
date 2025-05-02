using AutoMapper;
using Botanical.ViewModels.StoreViewModels;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Botanical.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WishlistController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            StoreVM storeVM = new StoreVM
            {
                Products = await _mapper.ProjectTo<GetProductDTO>(
             _context.Products.Where(p => !p.IsDeleted)
         ).ToListAsync(),

                Categories = await _mapper.ProjectTo<GetCategoryDTO>(
             _context.Categories.Where(p => !p.IsDeleted)
         ).ToListAsync(),
                Settings = await _mapper.ProjectTo<GetSettingsDTO>(
             _context.Settings.Where(s => !s.IsDeleted)
         ).ToListAsync()
            };
            return View(storeVM);
        }
    }
}
