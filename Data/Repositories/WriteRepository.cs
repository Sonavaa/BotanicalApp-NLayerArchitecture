﻿using Core;
using Data.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Common;

namespace Data.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entry = await Table.AddAsync(model);
            return entry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> model)
        {
            await Table.AddRangeAsync(model);
            return true;
        }

        public async Task<bool> Remove(string id)
        {
            T values = await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return Remove(values);
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entry = Table.Remove(model);
            return entry.State == EntityState.Deleted;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Update(T model)
        {
            EntityEntry entry = Table.Update(model);
            return entry.State == EntityState.Modified;
        }

        public bool UpdateRange(List<T> model)
        {
            Table.UpdateRange(model);

            return true;
        }
    }
}
