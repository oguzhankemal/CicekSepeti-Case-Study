using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Web.Api.Core;
using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;
using Web.Api.Web.Models;

namespace Web.Api.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository _repository;

        public CartController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Cart list
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            CartDto cart = _repository.List<Cart>().Select(CartDto.MapFromCartItem).FirstOrDefault();
            return View(cart);
        }

        /// <summary>
        /// This method creates initial Cart
        /// </summary>
        /// <returns></returns>
        public IActionResult InitCart()
        {
            int count = DatabaseInit.InitCart(_repository);
            return Ok(count);
        }

        /// <summary>
        /// This is the main method to add product to cart
        /// In this application, I assume there is one cart and we already know it
        /// And we also know the Id of the product we wanted to add
        /// This method only needs product Id
        /// !!! Important : Before call this method, you should call InitCart and InitProduct methods.
        /// By using the product id which is result of the InitProduct, you an call this method.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult AddProdutToCart(Guid productId)
        {
            var product = _repository.GetById<Product>(productId);
            if(product != null)
            {
            // Check if there is enough stock to add product
                if(product.StockControl(1))
                {
                    //Get default cart
                    //We assume there is only one cart
                    var cart = _repository.List<Cart>().FirstOrDefault();
                    if (cart != null)
                    {
                        //Insert Item to Cart
                        //If this product has already been added,It updated the quantity 
                        //If this product is not already added, it create new one
                        cart.InsertItemToCart(productId, 1);

                        //Destock the product quantity
                        product.Destock(1);

                        //Lastly update the cart
                        _repository.Update(cart);
                    }

                    return Json(true);
                }

                return Json("İstediğiniz miktarda ürün bulunmamaktadır.");
            }
            return Json("Ürün bulunamadı");
        }
    }
}