using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Converters
{
    class BoolToStyleConverter : IValueConverter
    {
        public Style? TrueStyle { get; set; }
        public Style? FalseStyle { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
        {
            if(value is bool isTrue)
            {
                return isTrue ? TrueStyle : FalseStyle;
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}
