using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Core.Entities;

namespace Web.Api.Test
{
    public class CartBuilder
    {
        Cart _cart = new Cart();
        public CartBuilder Constructer(string name)
        {
            _cart = Cart.Create(name);
            return this;
        }
        public CartBuilder Constructer(Guid id, string name)
        {
            _cart = Cart.Create(id,name);
            return this;
        }
        public CartBuilder AddToCart(Guid productId, int quantity)
        {
            _cart.InsertItemToCart(productId, quantity);
            return this;
        }
        public bool FindCartItem(Guid productId)
        {
            CartItem item = _cart.FindCartItem(productId);
            return (item ==null)?false: item.ProductId == productId;
        }
        public bool RemoveCartItem(Guid cartItemId)
        {
            return !_cart.RemoveCartItem(cartItemId).CartItems.Where(m=>m.Id== cartItemId).Any();
        }
        public CartBuilder AccumulateCartItemQuantity(Guid cartItemId, int quantity)
        {
            _cart = _cart.AccumulateCartItemQuantity(cartItemId,quantity);
            return this;
        }

        public Cart Build() => _cart;
    }
}
