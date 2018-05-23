using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Admin.ViewModel
{
    public class PriceConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is Int32)) // ellenőrizzük az értéket
                return Binding.DoNothing; // ha nem megfelelő, nem csinálunk semmit

            Int32 price = (Int32)value;
            return price + " Ft";
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is String)) // ellenőrizzük az értéket
                return DependencyProperty.UnsetValue; // ha nem megfelelő, nem tudjuk az értéket beállítani

            try
            {
                String priceString = value as String;
                Int32 price;
                price = Int32.Parse(priceString.Substring(0, priceString.Length - 3)); // figyelembe vesszük, hogy törtet is beírhat 
                if (price < 0) // negatív sem lehet az érték
                    return DependencyProperty.UnsetValue;

                return (Int32)price;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
