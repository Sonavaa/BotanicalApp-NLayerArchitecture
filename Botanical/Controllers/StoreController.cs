using AutoMapper;
using Botanical.ViewModels.HomeViewModels;
using Botanical.ViewModels.StoreViewModels;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Business.DTOs.Tag;
using Business.IServices;
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
        private readonly IProductServiceForPresentation _productServiceForPresentation;
        private readonly ICategoryService _categoryService;
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;

        public StoreController(AppDbContext context, IProductService productService, IProductServiceForPresentation productServiceForPresentation,
            ICategoryService categoryService, ISettingsService settingsService, IMapper mapper)
        {
            _context = context;
            _productService = productService;
            _productServiceForPresentation = productServiceForPresentation;
            _categoryService = categoryService;
            _settingsService = settingsService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 9;

            var query = _context.Products
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt);
            var allProducts = await _productService.GetAllProductAsync();

            int totalProducts = await query.CountAsync();

            var pagedProducts = await _mapper
                .ProjectTo<GetProductDTO>(query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize))
                .ToListAsync();

            var storeVM = new StoreVM
            {
                Products = pagedProducts,

                Categories = await _categoryService.GetAllCategoryAsync(),

                Settings = await _settingsService.GetAllSettings(),

                Tags = await _mapper.ProjectTo<GetProductTagDTO>(
                    _context.Tags.Where(s => !s.IsDeleted)).ToListAsync(),
                AllProductsCount = allProducts.Count(),

                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize)
            };

            return View(storeVM);
        }


        public async Task<IActionResult> Shop(string sort, string category, string tag, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            int pageSize = 9;

            var products = await _productServiceForPresentation.GetSortedProductsAsync(sort, category, tag, minPrice, maxPrice);

            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var allProducts = await _productService.GetAllProductAsync();

            var storeVM = new StoreVM
            {
                Products = pagedProducts,
                Categories = await _categoryService.GetAllCategoryAsync(),
                Settings = await _settingsService.GetAllSettings(),
                Tags = await _mapper.ProjectTo<GetProductTagDTO>(
                    _context.Tags.Where(s => !s.IsDeleted)).ToListAsync(),
                AllProductsCount = allProducts.Count(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(products.Count / (double)pageSize)
            };

            ViewBag.CurrentTag = tag;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSort = sort;

            if (products.Any())
            {
                ViewBag.MinPrice = products.Min(p => p.DiscountedPrice ?? p.Price);
                ViewBag.MaxPrice = products.Max(p => p.DiscountedPrice ?? p.Price);
            }
            else
            {
                ViewBag.MinPrice = 0;
                ViewBag.MaxPrice = 0;
            }

            return View("Index", storeVM);
        }


        [HttpPost]
        public async Task<IActionResult> GetFilteredProductsAsync(decimal minPrice, decimal maxPrice, int page = 1)
        {
            int pageSize = 9;

            var products = await _productService.GetAllProductAsync();
            var allProducts = await _productService.GetAllProductAsync();

            var filteredProducts = products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToList();

            var pagedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var storeVM = new StoreVM
            {
                Products = pagedProducts,
                Categories = await _categoryService.GetAllCategoryAsync(),
                Settings = await _settingsService.GetAllSettings(),
                Tags = await _mapper.ProjectTo<GetProductTagDTO>(
                    _context.Tags.Where(s => !s.IsDeleted)).ToListAsync(),
                AllProductsCount = allProducts.Count(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(filteredProducts.Count / (double)pageSize)
            };

            return View("Index", storeVM);
        }

    }
}
