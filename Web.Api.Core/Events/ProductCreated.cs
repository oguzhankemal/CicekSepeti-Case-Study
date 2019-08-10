using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Entities;

namespace Web.Api.Core.Events
{
    public class ProductCreated : DomainEvent
    {
        public Product Product { get; set; }

        public override void Flatten()
        {
            this.Args.Add("ProductId", this.Product.Id);
            this.Args.Add("ProductName", this.Product.Name);
            this.Args.Add("ProductDescription", this.Product.Description);
            this.Args.Add("ProductQuantity", this.Product.Quantity);
            this.Args.Add("ProductPrice", this.Product.Price);
        }
    }
}
