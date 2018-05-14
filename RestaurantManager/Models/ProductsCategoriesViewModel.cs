using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace RestaurantManager.Models
{
    public class ProductsCategoriesViewModel
    {
        public IEnumerable<Product> ProductList { get; set; }

        public IEnumerable<Category> CategoryList { get; set; }
    }
}
