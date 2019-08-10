using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Web.Api.Core.Entities;
using Web.Api.Core.Interfaces;
using Web.Api.Web.Filters;
using Web.Api.Web.Models;

namespace Web.Api.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly IRepository _repository;

        public CartItemsController(IRepository repository)
        {
            _repository = repository;
        }



        /// <summary>
        /// Api which return all cart item lists
        /// </summary>
        /// <returns></returns>
        /// GET: api/CartItems
        [HttpGet]
        public IActionResult List()
        {
            var items = _repository.List<CartItem>();
            return Ok(items);
        }

        /// <summary>
        /// It returns cart item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// GET: api/CartItems
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var item = CartItemDto.MapFromCartItem(_repository.GetById<CartItem>(id));
            return Ok(item);
        }

        /// <summary>
        /// This is the main Api to add product to cart
        /// In this application, I assume there is one cart and we already know it
        /// And we also know the Id of the product we wanted to add
        /// This Api only needs product Id
        /// !!! Important : Before call this api, you should call AddTestCart and AddTestProduct.
        /// By using the product id which is result of the AddTestProduct, you an call Post Api.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Guid productId)
        {
            var product = _repository.GetById<Product>(productId);
            if (product != null)
            {
                // Check if there is enough stock to add product
                if (product.StockControl(1))
                {
                    //Get default cart
                    //We assume there is only one cart
                    var cart = _repository.List<Cart>().FirstOrDefault();
                    if (cart != null)
                    {
                        //Insert Item to Cart
                        //If this product has already been added,It updated the quantity 
                        //If this product is not already added, it create new one
                        cart.InsertItemToCart(productId, 1);

                        //Destock the product quantity
                        product.Destock(1);

                        //Lastly update the cart
                        _repository.Update(cart);
                    }

                    return Ok(true);
                }

                return NotFound("İstediğiniz miktarda ürün bulunmamaktadır.");
            }
            return NotFound("Ürün bulunamadı");
        }

        /// <summary>
        /// This api is written for testing purpose
        /// In creates the new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("AddTestProduct")]
        public IActionResult AddTestProduct([FromBody]ProductDto product)
        {
            Product newProduct = Product.Create(product.Id, product.Name, product.Description, product.Quantity, product.Price);

            var newlyAdded = _repository.Add(newProduct);

            if (newlyAdded != null)
            {
                return Ok(ProductDto.MapFromProductItem(newlyAdded));
                
            }
            return NotFound(null);
        }

        /// <summary>
        /// This api is written for testing purpose
        /// It creates the new cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost("AddTestCart")]
        public IActionResult AddTestProduct([FromBody]CartDto cart)
        {
            Cart newCart = Cart.Create(cart.Id, cart.CartName);

            var newlyAdded = _repository.Add(newCart);

            if (newlyAdded != null)
            {
                return Ok(CartDto.MapFromCartItem(newlyAdded));

            }
            return NotFound(null);
        }
    }
}