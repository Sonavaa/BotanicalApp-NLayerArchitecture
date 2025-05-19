using Core.Entities;
using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IWishlistItemReadRepository : IReadRepository<WishListItem>
    {
        Task<WishListItem> GetByUserIdAndProductIdAsync(string appUserId, Guid productId);
    }
}
