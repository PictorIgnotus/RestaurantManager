using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Admin.ViewModel
{
    public class CategoryTypeConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == null || !(value is CategoryType))
                return Binding.DoNothing;

            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> categoryNames = (parameter as IEnumerable<String>).ToList();
            Int32 index = (Int32)value;

            if (index < 0 || index >= categoryNames.Count)
                return Binding.DoNothing;

            return categoryNames[index];
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == null || !(value is String))
                return DependencyProperty.UnsetValue;

            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> categoryNames = (parameter as IEnumerable<String>).ToList();
            String name = (String)value;

            if (!categoryNames.Contains(name))
                return DependencyProperty.UnsetValue;

            return (CategoryType)categoryNames.IndexOf(name);

        }
    }
}
