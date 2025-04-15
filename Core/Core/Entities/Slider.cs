using Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Slider : BaseAuditable
    {
        public string Title { get; set; } = null!;
        public string? SubTitle { get; set; }
        public string Description { get; set; } = null!;
        public string? ImagePath { get; set; }
    }
}
