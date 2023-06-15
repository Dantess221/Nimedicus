using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Nimedicus.Utils.AttachedBehaviours;

namespace Nimedicus.Controls.SVGControls
{
    /// <summary>
    /// Логика взаимодействия для RecolorableImage.xaml
    /// </summary>
    public partial class RecolorableImage : BaseRecolorableImage
    {
        private static readonly Dictionary<UriColorShift, ImageSource> _cachedColorShiftObjects = new Dictionary<UriColorShift, ImageSource>();

        #region ColorShift dpd w OnChanged

        public static readonly DependencyProperty ColorShiftProperty = DependencyProperty.Register(
           "ColorShift", typeof(Color), typeof(RecolorableImage), new PropertyMetadata(default(Color), OnColorShiftChanged));

        public Color ColorShift
        {
            get { return (Color)GetValue(ColorShiftProperty); }
            set { SetValue(ColorShiftProperty, value); }
        }
        private static void OnColorShiftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as RecolorableImage;
            if (zis == null) return;

            zis.ShiftColor();
        }

        public static readonly DependencyProperty ColorShiftBrushProperty = DependencyProperty.Register(
           "ColorShiftBrush", typeof(SolidColorBrush), typeof(RecolorableImage), new PropertyMetadata(default(SolidColorBrush), OnBrushShiftChanged));

        public SolidColorBrush ColorShiftBrush
        {
            get { return (SolidColorBrush)GetValue(ColorShiftBrushProperty); }
            set { SetValue(ColorShiftBrushProperty, value); }
        }

        private static void OnBrushShiftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as RecolorableImage;
            if (zis == null) return;

            zis.ShiftBrush();
        }

        #endregion

        public RecolorableImage()
        {
            InitializeComponent();
        }

        protected override void ShiftColor()
        {
            ResetToDefaultImgeSource();
            if (IsColorShift)
            {
                if (Image?.Source == null) return;

                try
                {
                    var uri = (Image.Source).ToString();
                    var imageColorShift = new UriColorShift(uri, ColorShift);
                    if (_cachedColorShiftObjects.ContainsKey(imageColorShift))
                    {
                        Image.Source = _cachedColorShiftObjects[imageColorShift];
                    }
                    else
                    {
                        ImageRecolor.ChangeImageColor(Image, ColorShift);
                        _cachedColorShiftObjects.Add(imageColorShift, Image.Source);
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        ImageRecolor.ChangeImageColor(Image, ColorShift);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        protected void ShiftBrush()
        {
            if (ColorShiftBrush != null)
            {
                ColorShift = ColorShiftBrush.Color;
                ShiftColor();
            }
        }

        private struct UriColorShift
        {
            public string Uri { get; }
            public Color ColorShift { get; }

            public UriColorShift(string uri, Color colorShift)
            {
                Uri = uri;
                ColorShift = colorShift;
            }

            public override bool Equals(object obj)
            {
                if (obj is UriColorShift)
                {
                    var anotherInstnce = (UriColorShift)obj;
                    return base.Equals(obj) || (Uri.Equals(anotherInstnce.Uri) && ColorShift.Equals(anotherInstnce.ColorShift));
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Uri.GetHashCode() ^ ColorShift.GetHashCode();
            }
        }
    }
}
