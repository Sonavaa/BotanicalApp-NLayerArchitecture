﻿using Business.DTOs.Products;
using Business.IServices;
using Business.Services;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Botanical.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServiceForPresentation _productServiceForPresentation;

        public ProductController(IProductServiceForPresentation productServiceForPresentation)
        {
            _productServiceForPresentation = productServiceForPresentation;       
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await _productServiceForPresentation.ProductDetail(id);
            if (product == null)
                return NotFound();

            return View(product); 
        }


        [HttpPost]
        public IActionResult DailyDeal()
        {
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
            var endDate = startDate.AddDays(1);

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View("Index", "Home");
        }

        public async Task<IActionResult> Search(string search)
        {
            var products = await _productServiceForPresentation.Search(search);

            return PartialView("_SearchPartial", products ?? new List<GetProductDTO>());
        }
    }
}
