using System.Linq;
using Web.Api.Core.Entities;
using Web.Api.Infrastructure.Data;

namespace Web.Api.Web
{
    public static class SeedData
    {
        /// <summary>
        /// For testing, this seed creates two product and one cart. It also adds products to cart.
        /// </summary>
        /// <param name="dbContext"></param>
        public static void PopulateTestData(ApiDbContext dbContext)
        {
            var product = dbContext.Products;
            foreach (var item in product)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.Products.Add(Product.Create("Meyve Sepeti", "Mevsimlik Meyve Sepeti", 7, 40D));
            dbContext.Products.Add(Product.Create("Çikolata Topları", "40'lı Çikolata Topları", 10, 80D));
            
            var carts = dbContext.Carts;
            foreach (var item in carts)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.Carts.Add(Cart.Create("Sepet"));

            dbContext.SaveChanges();
            var cart = dbContext.Carts.FirstOrDefault();
            product = dbContext.Products;
            foreach (var item in product)
            {
                cart.InsertItemToCart(item.Id, 1);
            }

            dbContext.Carts.Update(cart);
            dbContext.SaveChanges();

            var cartItems = dbContext.CartItems;
        }

    }
}

