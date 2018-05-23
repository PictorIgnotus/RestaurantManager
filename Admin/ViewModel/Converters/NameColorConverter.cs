using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Admin.ViewModel
{
    public class NameColorConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is Boolean)) // ellenőrizzük az értéket
                return Binding.DoNothing; // ha nem megfelelő, nem csinálunk semmit
            Boolean nameAlreadyExists = (Boolean)value;
            return nameAlreadyExists ? ConsoleColor.Red : ConsoleColor.Gray;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
