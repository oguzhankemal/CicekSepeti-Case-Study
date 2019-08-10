using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Core.Events;
using Xunit;

namespace Web.Api.Test.Core.Entities
{
    public class ProductTest
    {
        [Fact]
        public void StockTrueControl()
        {
            var item = new ProductBuilder().Constructer("20 Gül", "20'li gül demeti", 10, 50D);
            
            Assert.True(item.StockControl(5));
        }
        [Fact]
        public void StockFalseControl()
        {
            var item = new ProductBuilder().Constructer("20 Gül", "20'li gül demeti", 10, 50D);

            Assert.False(item.StockControl(15));
        }
        [Fact]
        public void DestockTrueControl()
        {
            var item = new ProductBuilder().Constructer("20 Gül", "20'li gül demeti", 10, 50D);

            Assert.Equal(5,item.Destock(5));
        }
        [Fact]
        public void DestockFalseControl()
        {
            var item = new ProductBuilder().Constructer("20 Gül", "20'li gül demeti", 10, 50D);

            Assert.NotEqual(4, item.Destock(5));
        }

        [Fact]
        public void RaiseProductCreatedEvent()
        {
            var item = new ProductBuilder().Constructer("20 Gül", "20'li gül demeti", 10, 50D);

            item.CreatedEvent();

            Assert.Single(item.Events);
            Assert.IsType<ProductCreated>(item.Events.First());
        }
    }
}
