using Core.Entities;
using Core.Repositories;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductTagWriteRepository : WriteRepository<ProductTag>, IProductTagWriteRepository
    {
        public ProductTagWriteRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<ProductTag> GetAll(bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<ProductTag> GetByIdAsync(string id, bool tracking = true)
        {
            throw new NotImplementedException();
        }
    }
}
