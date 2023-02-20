using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace Libery_Frontend.SecondModels
{
    public class StringConverter : IValueConverter
    {
        #region IValueConverter implementation
        private const int MaxLength = 100;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            //var longstring = (string)value;
            //put your custom formatting here
            return string.Concat(value.ToString().Substring(0, MaxLength-1), "...");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
