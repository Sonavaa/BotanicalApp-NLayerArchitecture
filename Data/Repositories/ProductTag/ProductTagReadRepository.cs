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
    public class ProductTagReadRepository : ReadRepository<ProductTag>, IProductTagReadRepository
    {
        public ProductTagReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
