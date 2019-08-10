using Newtonsoft.Json;
using System;
using Web.Api.Core.Entities;

namespace Web.Api.Web.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public CartDto Cart { get; set; }
        public Guid CartId { get; set; }
        public ProductDto Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public static CartItemDto MapFromCartItem(CartItem cart)
        {
            return new CartItemDto()
            {
                Id = cart.Id,
                //Cart = CartDto.MapFromCartItem(cart.Cart),
                CartId = cart.CartId,
                Product = ProductDto.MapFromProductItem(cart.Product),
                ProductId = cart.ProductId,
                Quantity = cart.Quantity
            };
        }
    }
}
