using System;
using System.Globalization;
using Xamarin.Forms;

namespace Libery_Frontend.SecondModels
{
    public class BoolConverter : IValueConverter
    {
        #region IValueConverter implementation

        CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string answer = null;

            if (value == null)
            {
                answer = "Information ej tillgänglig";
                return answer;
            }

            var yesOrNo = (bool)value;

            if (yesOrNo == true) answer = $"Ja";

            else if (yesOrNo == false) answer = $"Nej";

            //put your custom formatting here
            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
