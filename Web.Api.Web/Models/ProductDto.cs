using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Web.Api.Core.Entities;

namespace Web.Api.Web.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public static ProductDto MapFromProductItem(Product item)
        {
            return new ProductDto()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price
            };
        }
    }
}
