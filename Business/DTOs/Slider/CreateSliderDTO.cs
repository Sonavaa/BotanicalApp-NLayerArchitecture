using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Slider
{
    public class CreateSliderDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? SubTitle { get; set; }
        public string Description { get; set; } = null!;
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
