using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence
{
     public class Product
    {
        public Product()
        {

        }
        public Product(string name, CategoryType category, Int32 price)
        {
            Name = name;
            Category = category;
            Price = price;
            Description = "";
            Hot = false;
            Vegetarian = true;
            SaleNumber = 0;
        }

        [Key]
        public Int32 Id { get; set; }

        [MaxLength(50)]
        public String Name { get; set; }

        public CategoryType Category { get; set; }

        [Range(0, 12000)]
        public Int32 Price { get; set; }

        public String Description { get; set; }

        public Int32 SaleNumber { get; set; }

        public Boolean Hot { get; set; }

        public Boolean Vegetarian { get; set; }
    }
}
