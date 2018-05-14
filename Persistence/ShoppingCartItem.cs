using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class ShoppingCartItem
    {
        public Int32 ShoppingCartItemId { get; set; }

        public Product Item { get; set; }

        public Int32 Amount { get; set; }

        public String ShoppingCartId { get; set; }
    }
}
