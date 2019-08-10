using System;
using Web.Api.Core.Events;

namespace Web.Api.Core.Entities
{
    //Product class has Stock Control, Destock and Created Event methods
    public class Product : BaseEntity
    {
        public static Product Create(string name, string description, int quantity, double price)
        {
            return Create(Guid.NewGuid(), name, description, quantity, price);
        }

        public static Product Create(Guid id, string name, string description, int quantity, double price)
        {
            Product product = new Product()
            {
                Id = id,
                Name = name,
                Description = description,
                Quantity = quantity,
                Price = price,

                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            return product;
        }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get; private set; }

        //Checks if there is a enough stock
        public bool StockControl(int quantity)
        {
            return Quantity >= quantity;
        }

        //Decrease the stock quantity
        public int Destock(int quantity)
        {
            Quantity -= quantity;
            return Quantity;
        }

        //Create Domain Events when product created
        public void CreatedEvent()
        {
            Events.Add(new ProductCreated() { Product = this });
        }
    }
}
