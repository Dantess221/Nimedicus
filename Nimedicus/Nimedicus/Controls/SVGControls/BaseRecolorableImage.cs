using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Nimedicus.Controls.SVGControls
{
    public abstract class BaseRecolorableImage : UserControl
    {
        #region Image dpd

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(Image), typeof(BaseRecolorableImage), new PropertyMetadata(default(Image)));

        public Image Image
        {
            get { return (Image)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        #endregion

        #region Source dpd w OnChanged

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(BaseRecolorableImage), new PropertyMetadata(default(ImageSource), OnSourceChanged));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as BaseRecolorableImage;
            if (zis == null) return;

            zis.ShiftColor();
        }

        #endregion

        #region Stretch dpd w static ChangedCallback and Virtual OnChanged

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof(Stretch?), typeof(BaseRecolorableImage), new PropertyMetadata(null, StretchChangedCallback));

        public Stretch? Stretch
        {
            get { return (Stretch?)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        private static void StretchChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as BaseRecolorableImage;
            if (zis?.Stretch == null) return;

            zis.OnStretchChanged(e);
        }
        protected virtual void OnStretchChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region IsColorShift dpd w OnChanged

        public static readonly DependencyProperty IsColorShiftProperty = DependencyProperty.Register(
            "IsColorShift", typeof(bool), typeof(BaseRecolorableImage), new PropertyMetadata(default(bool), OnIsColorShift));

        public bool IsColorShift
        {
            get { return (bool)GetValue(IsColorShiftProperty); }
            set { SetValue(IsColorShiftProperty, value); }
        }
        private static void OnIsColorShift(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as BaseRecolorableImage;
            if (zis == null) return;

            zis.ShiftColor();
        }

        #endregion

        protected abstract void ShiftColor();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Image = Template.FindName("img", this) as Image;
            ResetToDefaultImgeSource();
            ShiftColor();
        }

        protected void ResetToDefaultImgeSource()
        {
            if (Image != null)
            {
                Image.Source = Source;
            }
        }
    }
}
