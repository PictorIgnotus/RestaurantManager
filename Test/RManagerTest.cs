using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using PersistenceManager;
using RestaurantManager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Admin.ViewModel;
using Admin.Model;
using Admin.Persistence;

namespace Test
{
    public class RManagerTest : IDisposable
    {
        private readonly RestaurantContext context;
        private readonly List<ProductDTO> prodcutDTO;
        private readonly List<OrderDTO> orderDTO;

        public RManagerTest()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseInMemoryDatabase("RManagerTest")
                .Options;

            context = new RestaurantContext(options);
            context.Database.EnsureCreated();

            var productData = new List<Product>
            {
                new Product
                {
                    Name = "TestHamburger",
                    Category = CategoryType.Hamburger,
                    Hot = true,
                    Vegetarian = false,
                    Description = "Test of hambureger",
                    Price = 1000,
                    SaleNumber = 4
                },
                new Product
                {
                    Name = "TestCoffe",
                    Category = CategoryType.Coffee,
                    Price = 1000,
                    SaleNumber = 2,
                }
            };

            context.Products.AddRange(productData);

            var orderData = new List<Order>
            {
                 new Order
                {
                    Name = "TestName1",
                    Address = "TestAddress1",
                    PhoneNumber = "1234567",
                    ShoppingCartItems = new List<ShoppingCartItem>()
                    {
                        new ShoppingCartItem {
                            Amount = 3,
                            ProductId = productData[0].Id,
                            Item = productData[0],
                        }
                    },
                    TransmittingDate = DateTime.Now.AddDays(-10),
                    CompletionDate = DateTime.Now.AddDays(-3)
                },
                new Order
                {
                    Name = "TestName2",
                    Address = "TestAddress2",
                    PhoneNumber = "7654321",
                    ShoppingCartItems = new List<ShoppingCartItem>()
                    {
                        new ShoppingCartItem {
                            Amount = 1,
                            ProductId = productData[1].Id,
                            Item = productData[1],

                        }
                    },
                    TransmittingDate = DateTime.Now.AddDays(-4),
                    CompletionDate = null
                }
            };

            context.Orders.AddRange(orderData);
            context.SaveChanges();

            prodcutDTO = productData.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                Hot = product.Hot,
                Vegetarian = product.Vegetarian,
                SaleNumber = product.SaleNumber

            }).ToList();


            orderDTO = orderData.Select(order => new OrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                CompletionDate = order.CompletionDate,
                TransmittingDate = order.TransmittingDate,
                Items = order.ShoppingCartItems.Select(sh => new ShoppingCartItemDTO
                {
                    Id = sh.Id,
                    Name = sh.Item.Name,
                    Price = sh.Item.Price,
                    Amount = sh.Amount,
                }),
                Price = 0,
            }).ToList();

        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetOrderTest()
        {
            var controller = new OrdersController(context);
            var result = controller.GetOrders();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(objectResult.Value);
        }

        [Fact]
        public void GetProductTest()
        {
            var controller = new ProductsController(context);
            var result = controller.GetProducts();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(objectResult.Value);
        }

        [Fact]
        public void CreateProduct()
        {
            var newProduct = new ProductDTO
            {
                Name = "NewTest",
                Category = CategoryType.Pizza,
                Price = 1200,
                Description = "Test of pizza",
                Hot = false,
                Vegetarian = false,
                SaleNumber = 0

            };

            var controller = new ProductsController(context);
            var result = controller.PostProduct(newProduct);

            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            var model = Assert.IsAssignableFrom<ProductDTO>(objectResult.Value);

            Assert.Equal(prodcutDTO.Count + 1, context.Products.Count());
        }

        [Fact]
        public void UpdateOrder()
        {
            var order = orderDTO.FirstOrDefault(o => o.CompletionDate == null);
            order.CompletionDate = DateTime.Now;

            var controller = new OrdersController(context);
            var result = controller.PutOrder(order);

            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(orderDTO.Count, context.Products.Count());
        }
    }
}
