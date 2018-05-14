using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
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

    public class Category
    {
        public Int32 Id { get; set; }

        public CategoryType Type { get; set; }

        public String Name { get; set; }
    }
}
