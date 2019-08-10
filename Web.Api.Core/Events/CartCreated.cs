using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Entities;

namespace Web.Api.Core.Events
{
    public class CartCreated : DomainEvent
    {
        public Cart Cart { get; set; }

        public override void Flatten()
        {
            this.Args.Add("CartId", this.Cart.Id);
            this.Args.Add("CartName", this.Cart.CartName);
        }
    }
}
