using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class Order
    {
        public Order()
        {
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }
        public Int32 Id { get; set; }
        public String Name { get; set; }

        public String Address { get; set; }

        public String PhoneNumber { get; set; }

        public DateTime? TransmittingDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
