using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenceManager
{
    public enum CategoryType
    {
        Soup,
        Pizza,
        Hamburger,
        Sandwich,
        Coffee,
        SoftDrink
    }
    public class CategoryDTO
    {   
        public Int32 Id { get; set; }

        public CategoryType Type { get; set; }

        public String Name { get; set; }
    }
}
