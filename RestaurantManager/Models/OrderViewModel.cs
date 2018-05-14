using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManager.Models
{
    public class OrderViewModel
    {
        public String OrdererName { get; set; }
        public String OrdererAddress { get; set; }
        public String OrdererPhoneNumber { get; set; }
    }
}
