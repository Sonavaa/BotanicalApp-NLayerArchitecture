using Core;
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
    public class SubscriberReadRepository : ReadRepository<Subscriber>, ISubscriberReadRepository
    {
        private readonly AppDbContext _context;

        public SubscriberReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Subscribers.AnyAsync(x => x.Email == email);
        }
    }
}
