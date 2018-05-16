using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistence
{
    public class ShoppingCartItem
    {
        [Key]
        public Int32 Id { get; set; }

        public Int32 OrderId { get; set; }

        public Int32 ProductId { get; set; }

        public Product Item { get; set; }

        public Int32 Amount { get; set; }
    }
}
