using Nimedicus.Controls.SVGControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nimedicus.Controls.ColoredButtons
{
    /// <summary>
    /// Логика взаимодействия для SVGButton.xaml
    /// </summary>

    public partial class SVGButton : BaseColoredButton
    {
        private static RecolorableSVG _svgImage;

        public static readonly DependencyProperty DrawingProperty = DependencyProperty.Register(
            "Drawing", typeof(Drawing), typeof(SVGButton), new PropertyMetadata(default(Drawing)));

        public static readonly DependencyProperty ColorShiftBrushProperty = DependencyProperty.Register(
            "ColorShiftBrush", typeof(Brush), typeof(SVGButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(SVGButton), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SVGMarginProperty = DependencyProperty.Register(
            "SVGMargin", typeof(Thickness), typeof(SVGButton), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty SVGWidthProperty = DependencyProperty.Register(
           "SVGWidth", typeof(double), typeof(SVGButton), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty SVGHeightProperty = DependencyProperty.Register(
            "SVGHeight", typeof(double), typeof(SVGButton), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty DotMarginProperty = DependencyProperty.Register(
            "DotMargin", typeof(Thickness), typeof(SVGButton), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty IsDotContentProperty = DependencyProperty.Register(
            "IsDotContent", typeof(bool), typeof(SVGButton), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty DotContentProperty = DependencyProperty.Register(
            "DotContent", typeof(object), typeof(SVGButton), new PropertyMetadata(null));

        public object DotContent
        {
            get { return (object)GetValue(DotContentProperty); }
            set { SetValue(DotContentProperty, value); }
        }
        public bool IsDotContent
        {
            get { return (bool)GetValue(IsDotContentProperty); }
            set { SetValue(IsDotContentProperty, value); }
        }
        public Thickness DotMargin
        {
            get { return (Thickness)GetValue(DotMarginProperty); }
            set { SetValue(DotMarginProperty, value); }
        }

        public Brush ColorShiftBrush
        {
            get { return (Brush)GetValue(ColorShiftBrushProperty); }
            set { SetValue(ColorShiftBrushProperty, value); }
        }

        public Drawing Drawing
        {
            get { return (Drawing)GetValue(DrawingProperty); }
            set { SetValue(DrawingProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Thickness SVGMargin
        {
            get { return (Thickness)GetValue(SVGMarginProperty); }
            set { SetValue(SVGMarginProperty, value); }
        }

        public double SVGWidth
        {
            get { return (double)GetValue(SVGWidthProperty); }
            set { SetValue(SVGWidthProperty, value); }
        }

        public double SVGHeight
        {
            get { return (double)GetValue(SVGHeightProperty); }
            set { SetValue(SVGHeightProperty, value); }
        }

        public static readonly DependencyProperty HighlightedBackgroundOpacityProperty = DependencyProperty.Register(
            "HighlightedBackgroundOpacity", typeof(double), typeof(SVGButton), new PropertyMetadata(1d));

        public double HighlightedBackgroundOpacity
        {
            get { return (double)GetValue(HighlightedBackgroundOpacityProperty); }
            set { SetValue(HighlightedBackgroundOpacityProperty, value); }
        }

        public SVGButton()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _svgImage = Template.FindName("SvgImage", this) as RecolorableSVG;
        }
    }
}
