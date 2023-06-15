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
using Nimedicus.Model.DatabaseDataModels;

namespace Nimedicus.Utils.Converters
{
    public class UserFullNameConverter : MarkupExtension, IValueConverter
    {
        private static UserFullNameConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new UserFullNameConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;

            var baseUser = (BaseUser)value;

            return baseUser.LastName + " " + baseUser.Name + " " + baseUser.MiddleName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
