using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace Libery_Frontend.SecondModels
{
    public class DateTimeConverter : IValueConverter
    {
        #region IValueConverter implementation

        CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var datetime = (DateTime)value;
            //put your custom formatting here
            return datetime.ToLocalTime().ToString($"dddd, MMMM dd, yyyy",dateTimeLanguage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
