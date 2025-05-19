using Core.Entities;
using Core.Repositories;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class WishlistItemReadRepository : ReadRepository<WishListItem>, IWishlistItemReadRepository
    {
        private readonly AppDbContext _context;

        public WishlistItemReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WishListItem> GetByUserIdAndProductIdAsync(string appUserId, Guid productId)
        {
            return await _context.WishListItem
             .FirstOrDefaultAsync(w => w.AppUserId == appUserId && w.ProductId == productId);
        }
    }
}
