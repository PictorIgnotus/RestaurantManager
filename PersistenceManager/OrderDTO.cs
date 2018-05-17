using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenceManager
{
    public class OrderDTO
    {
        public Int32 Id { get; set; }

        public DateTime? TransmittingDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public Int32 Price { get; set; }

        public String Name { get; set; }

        public String Address { get; set; }

        public String PhoneNumber { get; set; }

        public IEnumerable<ShoppingCartItemDTO> Items {get; set;}
    }
}
