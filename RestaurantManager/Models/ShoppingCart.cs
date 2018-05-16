using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManager.Models
{
    public class ShoppingCart
    {
        private readonly RestaurantContext context;
        private const Int32 cartLimit = 20000;
        public Boolean LimitReached { get; set; }

        private ShoppingCart(RestaurantContext context)
        {
            this.context = context;
        }

        public String ShoppingCartId { get; set; }
        public IList<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            var currentContext = services.GetService<RestaurantContext>();

            String cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(currentContext) { ShoppingCartId = cartId };
        }

        public void AddToCart(Product Item, int amount)
        {
            return;
        }

        public Int32 RemoveFromCart(Product Item)
        {
            return 1;
        }

        public IList<ShoppingCartItem> GetShoppingCartItems()
        {
            return null;
        }

        public void ClearCart()
        {
            return;
        }

        public decimal GetShoppingCartTotal()
        {
            return 0;
        }

        public Boolean IsLimitReached(Int32 price)
        {
            return GetShoppingCartTotal() + price > cartLimit;
        }
    }
}

