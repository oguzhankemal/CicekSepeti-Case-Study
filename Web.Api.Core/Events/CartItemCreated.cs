using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Entities;

namespace Web.Api.Core.Events
{
    public class CartItemCreated : DomainEvent
    {
        public CartItem CartItem { get; set; }

        public override void Flatten()
        {
            this.Args.Add("CartItemId", this.CartItem.Id);
            this.Args.Add("CartItemCartId", this.CartItem.CartId);
            this.Args.Add("CartItemProductId", this.CartItem.ProductId);
            this.Args.Add("CartItemQuantity", this.CartItem.Quantity);
        }
    }
}
