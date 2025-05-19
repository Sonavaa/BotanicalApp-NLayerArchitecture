using Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IWishlistService
    {
        Task SyncCookieToWishlistAsync(AppUser appUser, IRequestCookieCollection cookies, IResponseCookies responseCookies);
         //Task<bool> AddToWishlistAsync(Guid wishlistId, ClaimsPrincipal user, IRequestCookieCollection requestCookies, IResponseCookies responseCookies);
    //Task<bool> RemoveFromWishlistAsync(Guid productId, ClaimsPrincipal user, IRequestCookieCollection requestCookies, IResponseCookies responseCookies);
    }
}
