using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Core.Entities;
using Web.Api.Core.Events;
using Xunit;

namespace Web.Api.Test.Core.Entities
{
    public class CartTest
    {
        private static string CartName = "Sepet";
        private static string ProductName = "Ürün";
        private static string Desc = "Açıklama";
        private static int Quantity = 10;
        private static double Price = 50D;
        [Fact]
        public void ConstructerTest()
        {
            var item = new CartBuilder().Constructer(CartName);
            
            Assert.True(item.Build().CartName == CartName);
        }
        [Fact]
        public void AddToCartTest()
        {
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            var cart = new CartBuilder().Constructer(CartName);
            cart = cart.AddToCart(product.Id, 1);
            Assert.Single(cart.Build().CartItems);
            Assert.IsType<CartItem>(cart.Build().CartItems.FirstOrDefault());
        }
        [Fact]
        public void FindCartItemTrueTest()
        {
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            var cart = new CartBuilder().Constructer(CartName);
            cart = cart.AddToCart(product.Id, 1);

            Assert.True(cart.FindCartItem(product.Id));
        }
        [Fact]
        public void FindCartItemFalseTest()
        {
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            var cart = new CartBuilder().Constructer(CartName);
            cart = cart.AddToCart(product.Id, 1);

            Assert.False(cart.FindCartItem(new Guid()));
        }
        [Fact]
        public void RemoveCartItemTrueTest()
        {
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            var cart = new CartBuilder().Constructer(CartName);
            cart = cart.AddToCart(product.Id, 1);
            CartItem item = cart.Build().CartItems.FirstOrDefault();

            Assert.Single(cart.Build().CartItems);
            Assert.True(cart.RemoveCartItem(item.Id));
            Assert.Empty(cart.Build().CartItems);
        }
        [Fact]
        public void RemoveCartItemFalseTest()
        {
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            var cart = new CartBuilder().Constructer(CartName);
            cart = cart.AddToCart(product.Id, 1);
            CartItem item = cart.Build().CartItems.FirstOrDefault();

            Assert.Single(cart.Build().CartItems);
            Assert.True(cart.RemoveCartItem(new Guid()));
            Assert.Single(cart.Build().CartItems);
        }

        [Fact]
        public void AccumulateCartItemQuantityTest()
        {
            var product = new ProductBuilder().Constructer(ProductName, Desc, Quantity, Price);
            var cart = new CartBuilder().Constructer(CartName);
            cart = cart.AddToCart(product.Id, 1);
            CartItem item = cart.Build().CartItems.FirstOrDefault();
            cart = cart.AccumulateCartItemQuantity(item.Id, 2);

            Assert.Single(cart.Build().CartItems);
            Assert.Equal(3,cart.Build().CartItems.FirstOrDefault().Quantity);
        }

        [Fact]
        public void RaiseCartCreatedEvent()
        {
            var item = new CartBuilder().Constructer(CartName);

            item.Build().CreatedEvent();

            Assert.Single(item.Build().Events);
            Assert.IsType<CartCreated>(item.Build().Events.First());
        }
    }
}
