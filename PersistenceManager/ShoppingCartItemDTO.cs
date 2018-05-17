using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenceManager
{
    public class ShoppingCartItemDTO
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public Int32 Price { get; set; }

        public Int32 Amount { get; set; }
    }
}
