using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class Category
    {
        public Int32 Id { get; set; }

        public CategoryType Type { get; set; }

        public String Name { get; set; }
    }
}
