using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using RestaurantManager.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantManager.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly ShoppingCart shoppingCart;

        public ShoppingCartController(RestaurantContext context, ShoppingCart shoppingCart) : base(context)
        {
            this.shoppingCart = shoppingCart;
        }
        public ViewResult Index()
        {
            var items = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = items;

            var sCVM = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal(),
                IsLimitReached = shoppingCart.LimitReached,
            };
            return View("SCart", sCVM);
        }

        public IActionResult AddToShoppingCart(Int32 productId)
        {
            var selectedProduct = context.Products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct != null)
            {
                shoppingCart.LimitReached = shoppingCart.IsLimitReached(selectedProduct.Price);
                if (!shoppingCart.LimitReached)
                   shoppingCart.AddToCart(selectedProduct, 1);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromShoppingCart(Int32 ItemId)
        {
            var selectedProduct = context.Products.FirstOrDefault(p => p.Id == ItemId);
            if (selectedProduct != null)
            {
                shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearShoppingCart()
        {
            shoppingCart.ClearCart();
            return RedirectToAction("Index");
        }

        public IActionResult OrderTransmission()
        {
            return View("OrderTransmission");
        }

        public IActionResult Transmission(OrderViewModel viewmodel)
        {
            Order order = new Order
            {
                Name = viewmodel.OrdererName,
                Address = viewmodel.OrdererAddress,
                PhoneNumber = viewmodel.OrdererPhoneNumber,
                ShoppingCartItems = shoppingCart.GetShoppingCartItems(),
            };
            foreach(var item in order.ShoppingCartItems)
            {
                var product = context.Products.SingleOrDefault(p => p.Id == item.Item.Id);
                if (product != null)
                    ++product.SaleNumber;
            }
            context.Orders.Add(order);
            context.SaveChanges();
            shoppingCart.ClearCart();
            return RedirectToAction("Index", "Home");
        }
    }
}
