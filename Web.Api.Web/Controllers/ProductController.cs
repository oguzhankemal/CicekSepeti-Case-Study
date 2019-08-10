using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core;
using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;
using Web.Api.Web.Models;

namespace Web.Api.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository _repository;

        public ProductController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Product list
        /// There is and Add button near the items.
        /// By clicking add button, you can easily add product to cart.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var items = _repository.List<Product>().Select(ProductDto.MapFromProductItem).ToList();
            return View(items);
        }

        /// <summary>
        /// This method creates initial Products
        /// </summary>
        /// <returns></returns>
        public IActionResult InitProduts()
        {
            int count = DatabaseInit.InitProdut(_repository);
            return Ok(count);
        }
    }
}