using Core.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Setting : BaseAuditable
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImgPath { get; set; }
    }
}
