using Botanical.ViewModels.WishlistViewModels;
using Business.IServices;
using Core.Entities;
using Core.Entities.Identity;
using Core.Repositories;
using Data.Repositories;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Business.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductService _productService;
        private readonly IWishlistItemReadRepository _wishlistItemReadRepository;
        private readonly IWishlistItemWriteRepository _wishlistItemWriteRepository;
        private readonly UserManager<AppUser> _userManager;

        public WishlistService(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
             IProductService productService,
            IWishlistItemReadRepository wishlistItemReadRepository,
            IWishlistItemWriteRepository wishlistItemWriteRepository,
            UserManager<AppUser> userManager)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productService = productService;
            _wishlistItemReadRepository = wishlistItemReadRepository;
            _wishlistItemWriteRepository = wishlistItemWriteRepository;
            _userManager = userManager;
        }
        public async Task SyncCookieToWishlistAsync(AppUser appUser, IRequestCookieCollection cookies, IResponseCookies responseCookies)
        {
            var wishlistJson = cookies["WishlistItems"];
            if (wishlistJson == null || appUser == null)
                return;

            var wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistJson);

            foreach (var item in wishlistItems)
            {
                var existingItem = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, item.Id);
                if (existingItem == null)
                {
                    await _wishlistItemWriteRepository.AddAsync(new WishListItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = item.Id,
                        AppUserId = appUser.Id,
                        IsDeleted = false
                    });
                }
            }

            await _wishlistItemWriteRepository.SaveChangeAsync();
            responseCookies.Delete("WishlistItems");
        }

        //     public async Task<bool> AddToWishlistAsync(Guid wishlistId, ClaimsPrincipal user, IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        //{
        //        var products = await _productService.GetAllProductAsync();
        //        if (!products.Any(p => p.Id == wishlistId))
        //        {
        //             throw new Exception();
        //        }

        //        var wishlistItemsJson = requestCookies["WishlistItems"];
        //    var wishlistItems = new List<WishlistItemViewModel>();
        //    if (!string.IsNullOrEmpty(wishlistItemsJson))
        //    {
        //        wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistItemsJson);
        //    }

        //    var isAuthenticated = user.Identity.IsAuthenticated;
        //    AppUser appUser = null;

        //    if (isAuthenticated)
        //    {
        //        appUser = await _userManager.GetUserAsync(user);
        //    }

        //    if (isAuthenticated && appUser != null)
        //    {
        //        // Move cookie items to DB if any
        //        foreach (var item in wishlistItems)
        //        {
        //            var exists = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, item.Id);
        //            if (exists == null)
        //            {
        //                await _wishlistItemWriteRepository.AddAsync(new WishListItem
        //                {
        //                    Id = Guid.NewGuid(),
        //                    ProductId = item.Id,
        //                    AppUserId = appUser.Id,
        //                    IsDeleted = false
        //                });
        //            }
        //        }

        //        await _wishlistItemWriteRepository.SaveChangeAsync();

        //        responseCookies.Delete("WishlistItems");

        //        // Add current product if not already in DB
        //        var existingCurrent = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, wishlistId);
        //        if (existingCurrent == null)
        //        {
        //            await _wishlistItemWriteRepository.AddAsync(new WishListItem
        //            {
        //                Id = Guid.NewGuid(),
        //                ProductId = wishlistId,
        //                AppUserId = appUser.Id,
        //                IsDeleted = false
        //            });
        //            await _wishlistItemWriteRepository.SaveChangeAsync();
        //        }
        //    }
        //    else
        //    {
        //        // Not authenticated: update cookie
        //        if (!wishlistItems.Any(p => p.Id == wishlistId))
        //        {
        //            wishlistItems.Add(new WishlistItemViewModel
        //            {
        //                Id = wishlistId,
        //                Count = 1
        //            });

        //            var updatedJson = JsonConvert.SerializeObject(wishlistItems);
        //            responseCookies.Append("WishlistItems", updatedJson, new CookieOptions
        //            {
        //                Expires = DateTimeOffset.Now.AddDays(30)
        //            });
        //        }
        //    }

        //        // Update product wishlist flag
        //        var product = await _productReadRepository.GetByIdAsync(wishlistId.ToString());
        //        if (product == null)
        //        {
        //            throw new Exception("Product not found");
        //        }
        //        product.IsInWishList = true;
        //    _productWriteRepository.Update(product);
        //    await _productWriteRepository.SaveChangeAsync();

        //    return true;
        //}

        //public async Task<bool> RemoveFromWishlistAsync(Guid productId, ClaimsPrincipal user, IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
        //{
        //    var isAuthenticated = user.Identity.IsAuthenticated;
        //    AppUser appUser = null;
        //    if (isAuthenticated)
        //    {
        //        appUser = await _userManager.GetUserAsync(user);
        //    }

        //    if (isAuthenticated && appUser != null)
        //    {
        //        var existingItem = await _wishlistItemReadRepository.GetByUserIdAndProductIdAsync(appUser.Id, productId);
        //        if (existingItem != null)
        //        {
        //            existingItem.IsDeleted = true;
        //            _wishlistItemWriteRepository.Update(existingItem);
        //            await _wishlistItemWriteRepository.SaveChangeAsync();
        //        }
        //    }
        //    else
        //    {
        //        var wishlistItemsJson = requestCookies["WishlistItems"];
        //        if (!string.IsNullOrEmpty(wishlistItemsJson))
        //        {
        //            var wishlistItems = JsonConvert.DeserializeObject<List<WishlistItemViewModel>>(wishlistItemsJson);
        //            var itemToRemove = wishlistItems.FirstOrDefault(p => p.Id == productId);

        //            if (itemToRemove != null)
        //            {
        //                wishlistItems.Remove(itemToRemove);

        //                if (wishlistItems.Any())
        //                {
        //                    var updatedJson = JsonConvert.SerializeObject(wishlistItems);
        //                    responseCookies.Append("WishlistItems", updatedJson, new CookieOptions
        //                    {
        //                        Expires = DateTimeOffset.Now.AddDays(30)
        //                    });
        //                }
        //                else
        //                {
        //                    responseCookies.Delete("WishlistItems");
        //                }
        //            }
        //        }
        //    }

        //    var product = await _productReadRepository.GetByIdAsync(productId.ToString());
        //    if (product != null)
        //    {
        //        product.IsInWishList = false;
        //        _productWriteRepository.Update(product);
        //        await _productWriteRepository.SaveChangeAsync();
        //    }

        //    return true;
        //}
    }
}
