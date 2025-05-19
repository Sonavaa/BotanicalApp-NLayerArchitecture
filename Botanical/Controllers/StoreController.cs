using AutoMapper;
using Botanical.Models;
using Botanical.ViewModels.HomeViewModels;
using Botanical.ViewModels.StoreViewModels;
using Botanical.ViewModels.WishlistViewModels;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Business.DTOs.Tag;
using Business.IServices;
using Business.Services;
using Core.Entities;
using Core.Entities.Identity;
using Core.Repositories;
using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;

namespace Botanical.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IProductReadRepository _ProductReadRepository;
        private readonly IProductWriteRepository _ProductWriteRepository;
        private readonly IProductServiceForPresentation _productServiceForPresentation;
        private readonly ICategoryService _categoryService;
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishlistItemReadRepository _wishlistItemReadRepository;
        private readonly IWishlistItemWriteRepository _wishlistItemWriteRepository;

        public StoreController(AppDbContext context, IProductService productService, IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository, IProductServiceForPresentation productServiceForPresentation,
            ICategoryService categoryService, ISettingsService settingsService, IMapper mapper, UserManager<AppUser> userManager,
            IWishlistItemReadRepository wishlistItemReadRepository, IWishlistItemWriteRepository wishlistItemWriteRepository)
        {
            _context = context;
            _productService = productService;
            _ProductReadRepository = productReadRepository;
            _ProductWriteRepository = productWriteRepository;
            _productServiceForPresentation = productServiceForPresentation;
            _categoryService = categoryService;
            _settingsService = settingsService;
            _mapper = mapper;
            _userManager = userManager;
            _wishlistItemReadRepository = wishlistItemReadRepository;
            _wishlistItemWriteRepository = wishlistItemWriteRepository;
        }
        public async Task<IActionResult> Index()
        {
            var storeVM = new StoreVM
            {
                Products = await _productService.GetAllProductAsync(),
                Categories = await _categoryService.GetAllCategoryAsync(),
                Settings = await _settingsService.GetAllSettings(),
                Tags = await _mapper.ProjectTo<GetProductTagDTO>(
                    _context.Tags.Where(s => !s.IsDeleted)).ToListAsync(),
                AllProductsCount = (await _productService.GetAllProductAsync()).Count(),
                CurrentPage = 1,
                TotalPages = 1
            };

            // Read cart from cookie
            var cartJson = HttpContext.Request.Cookies["CartItems"];
            if (cartJson is not null)
            {
                var cartItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(cartJson);

                foreach (var product in storeVM.Products)
                {
                    if (cartItems.Any(ci => ci.Id == product.Id))
                    {
                        product.IsInCart = true;
                    }
                }
            }

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

        [HttpGet]
        public async Task<IActionResult> AddToWishlist(Guid wishlistId)
        {
            var products = await _productService.GetAllProductAsync();
            if (!products.Any(p => p.Id == wishlistId))
            {
                return NotFound();
            }

            var wishlistItemsJSON = HttpContext.Request.Cookies["WishlistItems"];
            var wishlistItems = new List<WishlistItemViewModel>();
            if (wishlistItemsJSON is not null)
            {
                wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistItemsJSON);
            }

            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            AppUser appUser = isAuthenticated ? await _userManager.GetUserAsync(HttpContext.User) : null;

            if (isAuthenticated && appUser != null)
            {
                // STEP 1: Move items from cookie to DB if they exist
                foreach (var item in wishlistItems)
                {
                    var existingItem = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, item.Id);
                    if (existingItem == null)
                    {
                        var userWishlistItem = new WishListItem()
                        {
                            Id = Guid.NewGuid(),
                            ProductId = item.Id,
                            AppUserId = appUser.Id,
                            IsDeleted = false
                        };
                        await _wishlistItemWriteRepository.AddAsync(userWishlistItem);
                    }
                }

                // STEP 2: Save all pending changes from loop
                await _wishlistItemWriteRepository.SaveChangeAsync();

                // STEP 3: Clear the cookie, since data is now in DB
                HttpContext.Response.Cookies.Delete("WishlistItems");

                // STEP 4: Check if the current item is already in DB
                var existingCurrent = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, wishlistId);
                if (existingCurrent == null)
                {
                    var newWishlistItem = new WishListItem()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = wishlistId,
                        AppUserId = appUser.Id,
                        IsDeleted = false
                    };
                    await _wishlistItemWriteRepository.AddAsync(newWishlistItem);
                    await _wishlistItemWriteRepository.SaveChangeAsync();
                }
            }
            else
            {
                // User not authenticated, keep using cookie
                if (wishlistItems.All(p => p.Id != wishlistId))
                {
                    wishlistItems.Add(new WishlistItemViewModel
                    {
                        Id = wishlistId,
                        Count = 1
                    });

                    var updatedJson = JsonConvert.SerializeObject(wishlistItems);
                    HttpContext.Response.Cookies.Append("WishlistItems", updatedJson, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(30)
                    });
                }
            }

            // Update product wishlist flag
            var product = await _ProductReadRepository.GetByIdAsync(wishlistId.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            product.IsInWishList = true;
            _ProductWriteRepository.Update(product);
            await _ProductWriteRepository.SaveChangeAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromWishlistAsync(Guid wishlistId)
        {
            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            AppUser appUser = isAuthenticated ? await _userManager.GetUserAsync(HttpContext.User) : null;

            if (isAuthenticated && appUser is not null)
            {
                var existingItem = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, wishlistId);
                if (existingItem != null)
                {
                    existingItem.IsDeleted = true;
                    _wishlistItemWriteRepository.Update(existingItem);
                    await _wishlistItemWriteRepository.SaveChangeAsync();
                }
            }
            else
            {
                var wishlistItemsJSON = HttpContext.Request.Cookies["WishlistItems"];
                if (wishlistItemsJSON is not null)
                {
                    var wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistItemsJSON);
                    var itemToRemove = wishlistItems.FirstOrDefault(p => p.Id == wishlistId);

                    if (itemToRemove is not null)
                    {
                        wishlistItems.Remove(itemToRemove);

                        if (wishlistItems.Any())
                        {
                            var updatedJson = JsonConvert.SerializeObject(wishlistItems);
                            HttpContext.Response.Cookies.Append("WishlistItems", updatedJson, new CookieOptions
                            {
                                Expires = DateTimeOffset.Now.AddDays(30)
                            });
                        }
                        else
                        {
                            HttpContext.Response.Cookies.Delete("WishlistItems");
                        }
                    }
                }
            }

            var product = await _ProductReadRepository.GetByIdAsync(wishlistId.ToString());
            if (product != null)
            {
                product.IsInWishList = false;
                _ProductWriteRepository.Update(product);
                await _ProductWriteRepository.SaveChangeAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWishlistQuantity(Guid wishlistId, int count)
        {
            if (count < 1)
                return BadRequest("Invalid quantity");

            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            AppUser appUser = isAuthenticated ? await _userManager.GetUserAsync(HttpContext.User) : null;

            if (isAuthenticated && appUser is not null)
            {
                var existingItem = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, wishlistId);
                if (existingItem != null)
                {
                    existingItem.Count = count;
                    _wishlistItemWriteRepository.Update(existingItem);
                    await _wishlistItemWriteRepository.SaveChangeAsync();
                }
            }
            else
            {
                var wishlistItemsJSON = HttpContext.Request.Cookies["WishlistItems"];
                if (wishlistItemsJSON is not null)
                {
                    var wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistItemsJSON);
                    var item = wishlistItems.FirstOrDefault(p => p.Id == wishlistId);

                    if (item is not null)
                    {
                        item.Count = count;
                        var updatedJson = JsonConvert.SerializeObject(wishlistItems);
                        HttpContext.Response.Cookies.Append("WishlistItems", updatedJson, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(30)
                        });
                    }
                }
            }

            return Ok();
        }


        public IActionResult GetWishlistItems()
        {
            List<WishlistItemViewModel> wishlistItems = new List<WishlistItemViewModel>();

            var wishlistItemsJSON = HttpContext.Request.Cookies["WishlistItems"];

            if(wishlistItemsJSON is not null)
            {
                wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistItemsJSON);
            }

            return Ok(wishlistItems);
        }

        public async Task<IActionResult> Cart()
        {
            List<WishlistItemViewModel> cartItems = new();

            // Read from cookie (assuming unauthenticated users)
            var cartJson = HttpContext.Request.Cookies["CartItems"];
            if (cartJson is not null)
            {
                cartItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(cartJson);
            }

            // Optionally: If authenticated, override with database cart

            var allProducts = await _productService.GetAllProductAsync();
            var productsInCart = allProducts
                .Where(p => cartItems.Any(c => c.Id == p.Id))
                .ToList();

            // Set IsInCart flag
            foreach (var product in productsInCart)
            {
                product.IsInCart = true;
            }

            var storeVM = new StoreVM
            {
                Products = productsInCart,
                Categories = await _categoryService.GetAllCategoryAsync(),
                Settings = await _settingsService.GetAllSettings(),
                Tags = await _mapper.ProjectTo<GetProductTagDTO>(
                    _context.Tags.Where(s => !s.IsDeleted)).ToListAsync(),
                AllProductsCount = allProducts.Count(),
                CurrentPage = 1,
                TotalPages = 1
            };

            return View("Cart", storeVM);
        }

    }
}
