using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Common
{
    public interface ISoftDeletable
    {
        void SoftDelete();
        void Restore();

    }
}
