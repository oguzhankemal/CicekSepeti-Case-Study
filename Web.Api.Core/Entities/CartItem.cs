using System;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Api.Core.Events;

namespace Web.Api.Core.Entities
{
    /// <summary>
    /// Cart Item class is an bridge between product and cart
    /// It holds cart id, product id and quantity
    /// </summary>
    public class CartItem : BaseEntity
    {
        public static CartItem Create(Guid cartId, Guid productId, int quantity)
        {
            return Create(Guid.NewGuid(), cartId, productId, quantity);
        }

        public static CartItem Create(Guid id, Guid cartId, Guid productId, int quantity)
        {
            CartItem product = new CartItem()
            {
                Id = id,
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,

                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            return product;
        }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; private set; }
        public Guid CartId { get; private set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        /// <summary>
        /// Create domain event when cart item created
        /// </summary>
        public void CreatedEvent(CartItem cartItem)
        {
            Events.Add(new CartItemCreated() { CartItem = cartItem });
        }

        /// <summary>
        /// Increases the quantity with given quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public CartItem AccumulateQuantity(int quantity)
        {
            Quantity += quantity;
            return this;
        }

        /// <summary>
        /// It set the exact quantity with given quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public CartItem SetQuantity(int quantity)
        {
            Quantity = quantity;
            return this;
        }
    }
}
