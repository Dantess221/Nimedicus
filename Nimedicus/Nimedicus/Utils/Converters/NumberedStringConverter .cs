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
    public class NumberedStringConverter : MarkupExtension, IValueConverter
    {
        private static NumberedStringConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new NumberedStringConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList<string> list && list.Count > 0)
            {
                var numberedList = new List<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    string numberedString = $"{i + 1}. {list[i]}";
                    numberedList.Add(numberedString);
                }
                return numberedList;
            }
            return new List<string> { "-Не задано-" };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
