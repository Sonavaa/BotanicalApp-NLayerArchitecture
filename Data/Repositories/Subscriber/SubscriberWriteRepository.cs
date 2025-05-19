using Core;
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
    public class SubscriberWriteRepository : WriteRepository<Subscriber>, ISubscriberWriteRepository
    {
        public SubscriberWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
