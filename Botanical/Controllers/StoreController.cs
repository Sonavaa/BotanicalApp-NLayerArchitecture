using AutoMapper;
using Botanical.ViewModels.HomeViewModels;
using Botanical.ViewModels.StoreViewModels;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Business.DTOs.Tag;
using Business.Services;
using Core.Entities;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Botanical.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public StoreController(AppDbContext context, IProductService productService, IMapper mapper)
        {
            _context = context;
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 9;

            var query = _context.Products
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt); // Optional ordering

            int totalProducts = await query.CountAsync();

            var pagedProducts = await _mapper
                .ProjectTo<GetProductDTO>(query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize))
                .ToListAsync();

            var storeVM = new StoreVM
            {
                Products = pagedProducts,
                Categories = await _mapper.ProjectTo<GetCategoryDTO>(
                    _context.Categories.Where(p => !p.IsDeleted)).ToListAsync(),

                Settings = await _mapper.ProjectTo<GetSettingsDTO>(
                    _context.Settings.Where(s => !s.IsDeleted)).ToListAsync(),

                Tags = await _mapper.ProjectTo<GetProductTagDTO>(
                    _context.Tags.Where(s => !s.IsDeleted)).ToListAsync(),

                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize)
            };

            return View(storeVM);
        }


        public async Task<IActionResult> Shop(string sort)
        {
            var products = await _productService.GetAllProductAsync();

            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.DiscountedPrice ?? p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.DiscountedPrice ?? p.Price).ToList(),
                "top_rated" => products.OrderBy(p => p.CreatedAt).Where(p => p.IsInWishList).ToList(),
                "new_arr" => products.OrderByDescending(p => p.CreatedAt).ToList(),
                _ => products
            };

            var storeVM = new StoreVM
            {
                Products = products,
                Categories = await _mapper.ProjectTo<GetCategoryDTO>(
             _context.Categories.Where(p => !p.IsDeleted)
         ).ToListAsync(),
                Settings = await _mapper.ProjectTo<GetSettingsDTO>(
             _context.Settings.Where(s => !s.IsDeleted)
         ).ToListAsync()
            };

            ViewBag.CurrentSort = sort;
            return View("Index", storeVM);
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredProductsAsync(decimal minPrice, decimal maxPrice)
        {
            var products = await _productService.GetAllProductAsync();

            var filteredProducts = products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToList();

            var storeVM = new StoreVM
            {
                Products = filteredProducts,
                Categories = await _mapper.ProjectTo<GetCategoryDTO>(
                    _context.Categories.Where(p => !p.IsDeleted)
                ).ToListAsync(),
                Settings = await _mapper.ProjectTo<GetSettingsDTO>(
                    _context.Settings.Where(s => !s.IsDeleted)
                ).ToListAsync()
            };

            return View("Index", storeVM);
        }





    }
}
