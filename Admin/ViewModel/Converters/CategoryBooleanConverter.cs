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
    public class CategoryBooleanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == null || !(value is String))
                return DependencyProperty.UnsetValue;

            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> categoryNames = (parameter as IEnumerable<String>).ToList();
            String name = (String)value;

            if (!categoryNames.Contains(name))
                return DependencyProperty.UnsetValue;

            Boolean result = (CategoryType)categoryNames.IndexOf(name) != CategoryType.Coffee &&
                (CategoryType)categoryNames.IndexOf(name) != CategoryType.SoftDrink;

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
