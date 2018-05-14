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
            var shoppingCartItem = context.ShoppingCartItems
                .SingleOrDefault(s => s.Item.Id == Item.Id && s.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Item = Item,
                    Amount = 1,
                };

                context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                ++shoppingCartItem.Amount;
            }
            context.SaveChanges();
        }

        public Int32 RemoveFromCart(Product Item)
        {
            var shoppingCartItem = context.ShoppingCartItems
                .SingleOrDefault(s => s.Item.Id == Item.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            context.SaveChanges();

            return localAmount;
        }

        public IList<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                (ShoppingCartItems =
                    context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId)
                    .Include(c => c.Item)
                    .ToList());
        }

        public void ClearCart()
        {
            var cartItems = context.ShoppingCartItems
                .Where(s => s.ShoppingCartId == ShoppingCartId);

            context.ShoppingCartItems.RemoveRange(cartItems);

            context.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Item.Price * c.Amount).Sum();
            return total;
        }

        public Boolean IsLimitReached(Int32 price)
        {
            return GetShoppingCartTotal() + price > cartLimit;
        }
    }
}

