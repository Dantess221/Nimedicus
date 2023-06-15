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
    public class AnalysDateConverter : MarkupExtension, IValueConverter
    {
        private static AnalysDateConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new AnalysDateConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;

            var date = (DateTime)value;

            return "Аналізи та результативні висновки (" + date.ToString("dd.MM.yyyy") + ")";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
