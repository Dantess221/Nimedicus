using System.Windows;
using System.Windows.Controls;

namespace Nimedicus.Controls
{
    public partial class SVGDoubleTextControl : UserControl
    {
        public static readonly DependencyProperty DrawingProperty = DependencyProperty.Register(
            "Drawing", typeof(object), typeof(SVGDoubleTextControl), new PropertyMetadata(null));

        public object Drawing
        {
            get { return GetValue(DrawingProperty); }
            set { SetValue(DrawingProperty, value); }
        }

        public static readonly DependencyProperty UpperTextProperty = DependencyProperty.Register(
            "UpperText", typeof(string), typeof(SVGDoubleTextControl), new PropertyMetadata(string.Empty));

        public string UpperText
        {
            get { return (string)GetValue(UpperTextProperty); }
            set { SetValue(UpperTextProperty, value); }
        }

        public static readonly DependencyProperty DownTextProperty = DependencyProperty.Register(
            "DownText", typeof(string), typeof(SVGDoubleTextControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string DownText
        {
            get { return (string)GetValue(DownTextProperty); }
            set { SetValue(DownTextProperty, value); }
        }
        public SVGDoubleTextControl()
        {
            InitializeComponent();
        }
    }
}