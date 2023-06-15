using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace Nimedicus.Utils.Converters
{
    public class ReceiptDataConverter : MarkupExtension, IValueConverter
    {
        private static ReceiptDataConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ReceiptDataConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;

            if ((string)parameter == "id")
            {
                var id = (int)value; 
                return "Рецепт: №" + id.ToString("00000");
            }

            if ((string)parameter == "dataCreation")
            {
                var date = (DateTime)value;
                return "Призначення від " + date.ToString("dd.MM.yyyy");
            }

            if ((string)parameter == "dataExpiration")
            {
                var date = (DateTime)value;
                return "Дійсний до: " + date.ToString("dd.MM.yyyy");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
