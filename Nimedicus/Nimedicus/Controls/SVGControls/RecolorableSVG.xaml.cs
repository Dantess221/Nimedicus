using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Nimedicus.Utils.AttachedBehaviours;
using Nimedicus.Utils.ExtensionMethods;

namespace Nimedicus.Controls.SVGControls
{
    /// <summary>
    /// Логика взаимодействия для RecolorableSVG.xaml
    /// </summary>
    public partial class RecolorableSVG : BaseRecolorableImage
    {
        #region Drawing

        public static readonly DependencyProperty DrawingProperty = DependencyProperty.Register(
            "Drawing", typeof(Drawing), typeof(RecolorableSVG), new PropertyMetadata(default(Drawing), DrawingChangedCallback));

        public Drawing Drawing
        {
            get { return (Drawing)GetValue(DrawingProperty); }
            set { SetValue(DrawingProperty, value); }
        }

        private static void DrawingChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as RecolorableSVG;
            if (zis == null) return;

            if (zis.Drawing == null)
            {
                zis.Source = null;
                zis.ResetToDefaultImgeSource();
                zis._isRecolored = false;
                zis._drawingToRestore = null;
                //zis.OnSVGRendered?.Invoke(zis, EventArgs.Empty);
                return;
            }

            zis.OnDrawingChanged();
        }

        private void OnDrawingChanged()
        {
            RenderSVG();
        }

        #endregion // End - Drawing

        #region DisabledOpacity

        public static readonly DependencyProperty DisabledOpacityProperty = DependencyProperty.Register(
            "DisabledOpacity", typeof(double?), typeof(RecolorableSVG), new PropertyMetadata(null));

        public double? DisabledOpacity
        {
            get { return (double?)GetValue(DisabledOpacityProperty); }
            set { SetValue(DisabledOpacityProperty, value); }
        }

        #endregion // End - DisabledOpacity

        #region EnabledOpacity

        public static readonly DependencyProperty EnabledOpacityProperty = DependencyProperty.Register(
            "EnabledOpacity", typeof(double?), typeof(RecolorableSVG), new PropertyMetadata(null));

        public double? EnabledOpacity
        {
            get { return (double?)GetValue(EnabledOpacityProperty); }
            set { SetValue(EnabledOpacityProperty, value); }
        }

        #endregion // End - EnabledOpacity

        #region ColorShiftBrush dpd w OnChanged


        public static readonly DependencyProperty ColorShiftBrushProperty = DependencyProperty.Register(
            "ColorShiftBrush", typeof(Brush), typeof(RecolorableSVG), new PropertyMetadata(default(Brush), OnColorShiftBrushChanged));

        public Brush ColorShiftBrush
        {
            get { return (Brush)GetValue(ColorShiftBrushProperty); }
            set { SetValue(ColorShiftBrushProperty, value); }
        }

        private static void OnColorShiftBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as RecolorableSVG;
            if (zis == null) return;

            zis.ShiftColor();
        }

        #endregion

        #region ColorShiftRules

        public static readonly DependencyProperty IsColorShiftFillProperty = DependencyProperty.Register(
    "IsColorShiftFill", typeof(bool), typeof(RecolorableSVG),
    new PropertyMetadata(true, OnColorShiftRuleChanged));

        public bool IsColorShiftFill
        {
            get { return (bool)GetValue(IsColorShiftFillProperty); }
            set { SetValue(IsColorShiftFillProperty, value); }
        }

        public static readonly DependencyProperty IsColorShiftStrokeProperty = DependencyProperty.Register(
            "IsColorShiftStroke", typeof(bool), typeof(RecolorableSVG),
            new PropertyMetadata(true, OnColorShiftRuleChanged));

        public bool IsColorShiftStroke
        {
            get { return (bool)GetValue(IsColorShiftStrokeProperty); }
            set { SetValue(IsColorShiftStrokeProperty, value); }
        }

        public static readonly DependencyProperty IsColorShiftInvisibleProperty = DependencyProperty.Register(
            "IsColorShiftInvisible", typeof(bool), typeof(RecolorableSVG), new PropertyMetadata(false, OnColorShiftRuleChanged));

        public bool IsColorShiftInvisible
        {
            get { return (bool)GetValue(IsColorShiftInvisibleProperty); }
            set { SetValue(IsColorShiftInvisibleProperty, value); }
        }

        private static void OnColorShiftRuleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as RecolorableSVG;
            if (zis == null) return;

            if ((bool)e.OldValue == false && (bool)e.NewValue)
            {
                zis.ResetToDefaultImgeSource();
            }

            zis.ShiftColor();
        }

        #endregion

        protected override void OnStretchChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnStretchChanged(e);
            if (Drawing != null)
            {
                RenderSVG();
            }
        }

        public RecolorableSVG()
        {
            InitializeComponent();
            IsEnabledChanged += RecolorableSVG_IsEnabledChanged;
        }

        private void RecolorableSVG_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DisabledOpacity == null) return;

            if (IsEnabled)
            {
                Opacity = EnabledOpacity.GetValueOrDefault(1);
            }
            else
            {
                Opacity = DisabledOpacity.Value;
            }
        }

        public event EventHandler OnSVGRendered;
        private void RenderSVG()
        {
            //var settings = new WpfDrawingSettings();

            if (Drawing != null)
            {
                if (Drawing.IsFrozen)
                {
                    Source = new DrawingImage(Drawing.Clone());
                }
                else
                {
                    Source = new DrawingImage(Drawing);
                }
                ResolveStretch();
            }
            ResetToDefaultImgeSource();
            ShiftColor();
            OnSVGRendered?.Invoke(this, EventArgs.Empty);
        }

        private void ResolveStretch()
        {
            if (Stretch == null || Stretch.Value == System.Windows.Media.Stretch.None)
            {
                if (double.IsNaN(Height))
                {
                    Height = Source.Height;
                }

                if (double.IsNaN(Width))
                {
                    Width = Source.Width;
                }
            }
            if (Stretch != null && Stretch.Value != System.Windows.Media.Stretch.None)
            {
                Height = double.NaN;
                Width = double.NaN;
            }
        }

        private bool _isRecolorPending;
        private bool _isRecolored;
        private Drawing _drawingToRestore;

        protected override void ShiftColor()
        {
            if (_isRecolorPending || Image?.Source == null) return;

            if (IsColorShift)
            {
                if (!_isRecolorPending && ColorShiftBrush != null)
                {
                    _isRecolorPending = true;

                    if (!_isRecolored)
                    {
                        _drawingToRestore = ((DrawingImage)Image.Source).Drawing.CloneWithChildren();
                        _isRecolored = true;
                    }

                    ImageRecolor.ChangeSVGColor(Image, ColorShiftBrush, IsColorShiftFill, IsColorShiftStroke, IsColorShiftInvisible);

                    _isRecolorPending = false;
                }
            }
            else if (_isRecolored)
            {
                _isRecolored = false; // It should be set before actualy restoring because ShiftColor will be called via OnSourceChanged
                ((DrawingImage)Image.Source).Drawing = _drawingToRestore;
                _drawingToRestore = null;
            }
        }
    }
}
