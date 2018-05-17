using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using PersistenceManager;

namespace RestaurantManager.Models
{
    public class FilterCategoryViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public CategoryType Type { get; set; }
    }
}
