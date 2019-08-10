using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Web.Api.Core.Entities;

namespace Web.Api.Web.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CartDto
    {
        [JsonProperty]
        public Guid Id { get; set; }
        [Required]
        [JsonProperty]
        public string CartName { get; set; }
        public List<CartItemDto> CartItems { get; set; }

        public static CartDto MapFromCartItem(Cart item)
        {
            return new CartDto()
            {
                Id = item.Id,
                CartName = item.CartName,
                CartItems = (item.CartItems == null) ? new List<CartItemDto>() : item.CartItems.Select(CartItemDto.MapFromCartItem).ToList()
            };
        }
    }
}
