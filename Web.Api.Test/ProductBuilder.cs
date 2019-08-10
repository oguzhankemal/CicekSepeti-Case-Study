using System;
using Web.Api.Core.Entities;

namespace Web.Api.Test
{
    public class ProductBuilder
    {
        public Product Constructer(string name, string description, int quantity, double price)
        {
            Product product = Product.Create(Guid.NewGuid(), name, description, quantity, price);
            return product;
        }
        public Product Constructer(Guid id, string name, string description, int quantity, double price)
        {
            Product product = Product.Create(id, name, description, quantity, price);
            return product;
        }
    }
}
