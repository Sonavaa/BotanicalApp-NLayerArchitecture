using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Common
{
    public class BaseEntity : ISoftDeletable
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }

        public virtual void Restore()
        {
            IsDeleted = false;
        }

        public virtual void SoftDelete()
        {
            IsDeleted = true;
        }
    }
}
