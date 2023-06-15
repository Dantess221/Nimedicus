using SharpVectors.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Nimedicus.Utils.Converters
{
    public class RecolorableSVGDrawingIdResolveConverter : MarkupExtension, IValueConverter
    {
        private static RecolorableSVGDrawingIdResolveConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new RecolorableSVGDrawingIdResolveConverter());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Drawing drawing)
            {
                var id = SvgObject.GetId(drawing);
                return id;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
