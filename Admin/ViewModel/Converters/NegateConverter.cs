using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Admin.ViewModel
{
    class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean original = (bool)value;
            return !original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean original = (bool)value;
            return !original;
        }
    }
}
