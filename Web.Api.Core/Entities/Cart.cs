using System;
using System.Collections.Generic;
using System.Linq;
using Web.Api.Core.Events;

namespace Web.Api.Core.Entities
{
    /// <summary>
    /// Cart class
    /// There are FindCartItem, InsertItemToCart, RemoveCartItem, AccumulateCartItemQuantity and CreatedEvent
    /// </summary>
    public class Cart : BaseEntity
    {
        public static Cart Create(string cartName)
        {
            return Create(Guid.NewGuid(), cartName);
        }

        public static Cart Create(Guid id, string cartName)
        {
            Cart cart = new Cart()
            {
                Id = id,
                CartName = cartName,

                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            return cart;
        }

        public virtual string CartName { get; set; }
        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();

        /// <summary>
        /// Finds cart item with product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public CartItem FindCartItem(Guid productId)
        {
            if (CartItems == null && CartItems.Count == 0)
                return null;
            var cartItem = CartItems.FirstOrDefault(x => x.ProductId == productId);
            return cartItem;
        }

        /// <summary>
        /// Insert Product to cart by product Id and quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public CartItem InsertItemToCart(Guid productId, int quantity)
        {
            //Cart item
            var cartItem = CartItems.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem == null)
                //If there is no item, it creates
                CartItems.Add(CartItem.Create(this.Id, productId, quantity));
            else
                //If there is, increase the quantity
                cartItem.AccumulateQuantity(quantity);
            return cartItem;
        }

        /// <summary>
        /// Remove product from cart by cart item Id
        /// </summary>
        /// <param name="cartItemId"></param>
        /// <returns></returns>
        public Cart RemoveCartItem(Guid cartItemId)
        {
            if (CartItems == null && CartItems.Count == 0)
                return null;
            CartItems = CartItems.Where(y => y.Id != cartItemId).ToList();
            return this;
        }

        /// <summary>
        /// Increases the cart item quantity with the given quantity
        /// It also checks if there is an item
        /// </summary>
        /// <param name="cartItemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Cart AccumulateCartItemQuantity(Guid cartItemId, int quantity)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Id == cartItemId);

            if (cartItem == null) throw new Exception($"Parça bulunamadı : #{cartItemId}");

            cartItem.AccumulateQuantity(quantity);
            return this;
        }

        /// <summary>
        /// Create domain event when cart created
        /// </summary>
        public void CreatedEvent()
        {
            Events.Add(new CartCreated() { Cart = this });
        }
    }
}
