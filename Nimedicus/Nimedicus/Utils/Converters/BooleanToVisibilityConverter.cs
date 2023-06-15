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
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static BooleanToVisibilityConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new BooleanToVisibilityConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string sParam)
            {
#if DEBUG
                if (sParam == "debug")
                    Debugger.Break();
#endif

                var parameters = sParam.Split(' ');

                if (parameters.Any(p => p.ToLowerInvariant() == "hidden"))
                {
                    if (parameters.Length > 1)
                    {
                        return (bool)value ? Visibility.Hidden : Visibility.Visible;
                    }

                    return (bool)value ? Visibility.Visible : Visibility.Hidden;
                }
            }

            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            if (value != null)
            {
                bool input = (null == parameter) ? (bool)value : !((bool)value);
                return (input) ? Visibility.Visible : Visibility.Collapsed;
            }

            return ((bool?)value != false) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if ((parameter as string)?.ToLower() == "or")
            {
                result = Array.Find(values, value => value is bool && (bool)value) != null;
            }
            else
            {
                result = Array.TrueForAll(values, value => value is bool && (bool)value);
            }



            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibilityToBooleanConverter : MarkupExtension, IValueConverter
    {
        private static VisibilityToBooleanConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new VisibilityToBooleanConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter != null)
            {
                return (Visibility)value != Visibility.Visible;
            }

            return (Visibility)value == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class InvertedBooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static InvertedBooleanToVisibilityConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new InvertedBooleanToVisibilityConverter());
        }
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            bool input = (null == parameter) ? (bool)value : !((bool)value);
            return (input) ? Visibility.Collapsed : Visibility.Visible;
            //return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class InvertVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static InvertVisibilityConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new InvertVisibilityConverter());
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                Visibility vis = (Visibility)value;
                return vis == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            }
            throw new InvalidOperationException("Converter can only convert to value of type Visibility.");
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new Exception("Invalid call - one way only");
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        private static InverseBooleanConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new InverseBooleanConverter());
        }
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            //if (targetType != typeof(bool))
            //throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion
    }

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ValueIsNullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static ValueIsNullToVisibilityConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ValueIsNullToVisibilityConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = value == null;
            var strVal = value as string;
            var hidden = parameter?.ToString() == "hidden";
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            if (strVal != null && strVal == "")
            {
                bValue = false;
            }

            bool output = (!hidden && parameter == null) ? bValue : !bValue;
            return output ? Visibility.Visible : hidden ? Visibility.Hidden : Visibility.Collapsed;
            //return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ValueIsNullToBooleanConverter : MarkupExtension, IValueConverter
    {
        private static ValueIsNullToBooleanConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ValueIsNullToBooleanConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = value == null;
            var strVal = value as string;
            var hidden = parameter?.ToString() == "hidden";
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            if (strVal != null && strVal == "")
            {
                bValue = false;
            }

            return (!hidden && parameter == null) ? bValue : !bValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ValueIsNotNullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static ValueIsNotNullToVisibilityConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ValueIsNotNullToVisibilityConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = value != null;
            var strVal = value as string;
            var hidden = parameter?.ToString() == "hidden";
            //reverse conversion (false=>Visible, true=>collapsed) on any given parameter
            if (strVal != null && strVal == "")
            {
                bValue = false;
            }

            bool output = (!hidden && parameter == null) ? bValue : !bValue;
            return output ? Visibility.Visible : hidden ? Visibility.Hidden : Visibility.Collapsed;
            //return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
