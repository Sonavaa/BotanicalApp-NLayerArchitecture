using AutoMapper;
using Business.DTOs.Settings;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace Botanical.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AboutController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var Settings = await _mapper.ProjectTo<GetSettingsDTO>(
           _context.Settings.Where(s => !s.IsDeleted)
       ).ToListAsync();

            return View(Settings);
        }
    }
}
