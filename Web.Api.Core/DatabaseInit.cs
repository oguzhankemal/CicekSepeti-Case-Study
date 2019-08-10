using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core
{
    public static class DatabaseInit
    {
        /// <summary>
        /// Creates three products and return count
        /// </summary>
        /// <param name="productRepository"></param>
        /// <returns></returns>
        public static int InitProdut(IRepository productRepository)
        {
            if (productRepository.List<Product>().Any()) return 0;

            Product p1 = Product.Create("20 Gül", "20'li gül demeti", 10, 50D);
            productRepository.Add(p1);p1.CreatedEvent();
            Product p2 = Product.Create("40 Gül", "40'li gül demeti", 10, 90D);
            productRepository.Add(p2); p2.CreatedEvent();
            Product p3 = Product.Create("Papatya", "Bir demet papatya", 10, 70D);
            productRepository.Add(p3); p3.CreatedEvent();

            return productRepository.List<Product>().Count;
        }

        /// <summary>
        /// It creates a cart and return 1
        /// </summary>
        /// <param name="cartRepository"></param>
        /// <returns></returns>
        public static int InitCart(IRepository cartRepository)
        {
            if (cartRepository.List<Cart>().Any()) return 0;

            Cart cart = cartRepository.Add(new Cart
            {
                CartName = "Sepet"
            });
            cart.CreatedEvent();
            return cartRepository.List<Cart>().Count;
        }
    }
}
