using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;
using Web.Api.Infrastructure.Data;
using Web.Api.Web;
using Web.Api.Web.Models;
using Xunit;

namespace Web.Api.Test.Integration.Web
{
    public class ApiCartItemsControllerList : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private ApiDbContext _dbContext;

        private static string CartName = "Sepet";
        private static string ProductName = "Ürün";
        private static string Desc = "Açıklama";
        private static int Quantity = 10;
        private static double Price = 50D;

        public ApiCartItemsControllerList(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        /// <summary>
        /// This test case use seed method and returns two cart items
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ReturnsTwoItems()
        {
            var response = await _client.GetAsync("/api/cartItems");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CartItem>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
        }

        /// <summary>
        /// This is the main test method
        /// ıt creates a product, cart and add product to cart by using api.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddToCartTest()
        {
            // new product
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            ProductDto productDto = ProductDto.MapFromProductItem(product);
            var jsonObject = JsonConvert.SerializeObject(productDto);
            HttpContent productContent = new StringContent(
                        jsonObject,
                    Encoding.UTF8,
                    "application/json");

            //creates product by using api
            var productResponse = await _client.PostAsync("/api/cartItems/AddTestProduct", productContent);

            Assert.Equal(HttpStatusCode.OK, productResponse.StatusCode);

            var productStringResponse = await productResponse.Content.ReadAsStringAsync();
            var productResult = JsonConvert.DeserializeObject<ProductDto>(productStringResponse);

            //In here we create cart by using repository instead of api.(I wanted to show both way)
            var repository = GetRepository();
            var cart = new CartBuilder().Constructer(CartName).Build();
            repository.Add(cart);

            //this is the main context which contains the Id of the product
            HttpContent content = new StringContent(
                        JsonConvert.SerializeObject(productResult.Id),
                    Encoding.UTF8,
                    "application/json");
            
            // Post Api which add product to cart
            var response = await _client.PostAsync("/api/cartItems", content);

            response.EnsureSuccessStatusCode();
            //Checks the http status
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(stringResponse);

            //Api returns true if the product successfully added
            Assert.True(result);
        }
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

        private ERepository GetRepository()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IEventDispatcher>();

            _dbContext = new ApiDbContext(options, mockDispatcher.Object);
            return new ERepository(_dbContext);
        }
    }
}
