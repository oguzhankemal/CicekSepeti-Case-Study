using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;
using Web.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Web.Api.Test.Integration.Data
{
    public class RepositoryShould
    {
        private ApiDbContext _dbContext;
        private static string CartName = "Sepet";
        private static string ProductName = "Ürün";
        private static string Desc = "Açıklama";
        private static int Quantity = 10;
        private static double Price = 50D;

        private static DbContextOptions<ApiDbContext> CreateNewContextOptions()
        {

            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApiDbContext>();
            builder.UseInMemoryDatabase("ciceksepeti")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        /// <summary>
        /// This test case adds a product and check it's Id
        /// </summary>
        [Fact]
        public void AddProductAndSetId()
        {
            var repository = GetRepository();
            var item = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);

            repository.Add(item);

            var newItem = repository.List<Product>().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id != null);
        }

        /// <summary>
        /// This test case adds a cart and check it's Id
        /// </summary>
        [Fact]
        public void AddCartAndSetId()
        {
            var repository = GetRepository();
            var item = new CartBuilder().Constructer(CartName).Build();

            repository.Add(item);

            var newItem = repository.List<Cart>().FirstOrDefault();

            Assert.Equal(item, newItem);
            Assert.True(newItem?.Id != null);
        }

        /// <summary>
        /// Add a product
        /// Adds a cart
        /// Adds product to cart
        /// Checks if product in the cart item is equal to created product
        /// Checks newly created item's Id is not null
        /// </summary>
        [Fact]
        public void AddToCart()
        {
            var repository = GetRepository();
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            repository.Add(product);

            var cart = new CartBuilder().Constructer(CartName).Build();
            repository.Add(cart);

            cart.InsertItemToCart(product.Id, 1);
            repository.Update(cart);
            
            var newItem = repository.List<Cart>().FirstOrDefault();

            Assert.Equal(product, newItem.CartItems.FirstOrDefault().Product);
            Assert.True(newItem?.Id != null);
        }
        

        private ERepository GetRepository()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IEventDispatcher>();

            _dbContext = new ApiDbContext(options, mockDispatcher.Object);
            return new ERepository(_dbContext);
        }
    }
}
