using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenceManager
{
    public class ProductDTO
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public CategoryType Category { get; set; }

        public Int32 Price { get; set; }

        public String Description { get; set; }

        public Int32 SaleNumber { get; set; }

        public Boolean Hot { get; set; }

        public Boolean Vegetarian { get; set; }
    }
}
