using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Nimedicus.Controls.ColoredButtons
{

    public abstract class BaseColoredButton : Button, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public BaseColoredButton()
        {
            Loaded += BaseColoredButton_Loaded;
            Unloaded += OnUnloaded;
        }

        #region TriggerSelf

        private bool _isTriggeredBySelf = false;
        public bool IsTriggeredBySelf
        {
            get => _isTriggeredBySelf;
            set
            {
                if (IsTriggeredBySelf != value)
                {
                    _isTriggeredBySelf = value;
                    OnPropertyChanged();
                }
            }
        }


        private void TriggerSelf(object sender, RoutedEventArgs e)
        {
            IsTriggeredBySelf = !IsTriggeredBySelf;
            IsTriggered = IsTriggeredBySelf;
        }

        public static readonly DependencyProperty IsTriggerSelfOnClickProperty = DependencyProperty.Register(
            "IsTriggerSelfOnClick", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(default(bool), IsTriggerSelfOnClickChangedCallback));

        public bool IsTriggerSelfOnClick
        {
            get { return (bool)GetValue(IsTriggerSelfOnClickProperty); }
            set { SetValue(IsTriggerSelfOnClickProperty, value); }
        }

        private static void IsTriggerSelfOnClickChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as BaseColoredButton;
            if (zis == null) return;

            zis.OnIsTriggerSelfOnClickChanged(e);
        }

        private void OnIsTriggerSelfOnClickChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsTriggerSelfOnClick)
            {
                Click += TriggerSelf;
            }
            else
            {
                Click -= TriggerSelf;
            }
        }

        public static readonly DependencyProperty UntriggerSelfOnUnloadProperty = DependencyProperty.Register(
            "UntriggerSelfOnUnload", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(false));

        public bool UntriggerSelfOnUnload
        {
            get { return (bool)GetValue(UntriggerSelfOnUnloadProperty); }
            set { SetValue(UntriggerSelfOnUnloadProperty, value); }
        }

        #endregion // End - TriggerSelf

        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName", typeof(string), typeof(BaseColoredButton), new PropertyMetadata(default(string), OnGroupNameChanged));

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        [ThreadStatic]
        private static Hashtable _groupNameToElements;
        private string _currentlyRegisteredGroupName = null;

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (BaseColoredButton)d;
            string newValue = e.NewValue as string;
            string groupName = button._currentlyRegisteredGroupName;

            if (!(newValue != groupName))
                return;

            if (!string.IsNullOrEmpty(groupName))
                Unregister(groupName, button);
            if (string.IsNullOrEmpty(newValue))
                return;
            Register(newValue, button);
        }

        private static void Register(string groupName, BaseColoredButton button)
        {
            if (_groupNameToElements == null)
                _groupNameToElements = new Hashtable(1);

            lock (_groupNameToElements)
            {
                var elements = (ArrayList)_groupNameToElements[(object)groupName];
                if (elements == null)
                {
                    elements = new ArrayList(1);
                    _groupNameToElements[(object)groupName] = (object)elements;
                }
                else
                    PurgeDead(elements, (object)null);
                elements.Add((object)new WeakReference((object)button));
            }

            button.GroupName = groupName;
            button._currentlyRegisteredGroupName = groupName;
        }

        private static void Unregister(string groupName, BaseColoredButton button)
        {
            if (_groupNameToElements == null)
                return;
            lock (_groupNameToElements)
            {
                ArrayList groupNameToElement = (ArrayList)_groupNameToElements[(object)groupName];
                if (groupNameToElement != null)
                {
                    PurgeDead(groupNameToElement, (object)button);
                    if (groupNameToElement.Count == 0)
                        _groupNameToElements.Remove((object)groupName);
                }
            }

            button.GroupName = null;
            button._currentlyRegisteredGroupName = null;
        }

        private static void PurgeDead(ArrayList elements, object elementToRemove)
        {
            int index = 0;
            while (index < elements.Count)
            {
                object target = ((WeakReference)elements[index]).Target;
                if (target == null || target == elementToRemove)
                    elements.RemoveAt(index);
                else
                    ++index;
            }
        }

        private bool _shouldUpdateTriggeredButtonGroup = true;
        private void UpdateTriggeredButtonGroup()
        {
            if (!_shouldUpdateTriggeredButtonGroup)
                return;

            string groupName = this.GroupName;
            if (!string.IsNullOrEmpty(groupName))
            {
                if (_groupNameToElements == null)
                    _groupNameToElements = new Hashtable(1);

                lock (_groupNameToElements)
                {
                    ArrayList groupNameToElement = (ArrayList)_groupNameToElements[(object)groupName];
                    int index = 0;
                    while (index < groupNameToElement.Count)
                    {
                        if (!(((WeakReference)groupNameToElement[index]).Target is BaseColoredButton target))
                        {
                            groupNameToElement.RemoveAt(index);
                        }
                        else
                        {
                            if (target != this)
                            {
                                if (target.IsTriggered)
                                    target.UntriggerButton();
                            }
                            ++index;
                        }
                    }
                }
            }
            //This code is from radiobuttons that kinda work without group name if name not specifed it tries to find neighbors
            //else
            //{
            //    DependencyObject parent = this.Parent;

            //    if (parent == null)
            //        return;

            //    foreach (object child in LogicalTreeHelper.GetChildren(parent))
            //    {
            //        if (child is BaseColoredButton button && button != this && string.IsNullOrEmpty(button.GroupName))
            //        {
            //            if (button.IsTriggered)
            //                button.UntriggerButton();
            //        }
            //    }
            //}
        }

        private void UntriggerButton()
        {
            _shouldUpdateTriggeredButtonGroup = false;
            IsTriggered = false;
            _shouldUpdateTriggeredButtonGroup = true;
        }

        #endregion // End - GroupName

        private void BaseColoredButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (PressedBackground == null)
            {
                PressedBackground = Application.Current?.MainWindow?.TryFindResource("ButtonBackgroundHover") as Brush;
            }
            if (PressedBorderBrush == null)
            {
                PressedBorderBrush = Application.Current?.MainWindow?.TryFindResource("ButtonBorderBrushHover") as Brush;
            }
            if (PressedForeground == null)
            {
                PressedForeground = Application.Current?.MainWindow?.TryFindResource("ButtonForeground") as Brush;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (UntriggerSelfOnUnload)
            {
                IsTriggered = false;
            }
        }

        public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
            "IsHighlighted", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(default(bool)));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty IsTriggeredProperty = DependencyProperty.Register(
            "IsTriggered", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(default(bool), IsTriggeredChangedCallback));

        public bool IsTriggered
        {
            get { return (bool)GetValue(IsTriggeredProperty); }
            set { SetValue(IsTriggeredProperty, value); }
        }

        private static void IsTriggeredChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zis = d as BaseColoredButton;
            if (zis == null) return;

            zis.OnIsTriggeredChanged(e);
        }

        private void OnIsTriggeredChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateTriggeredButtonGroup();
        }

        public static readonly DependencyProperty IsMouseOverHiglightEnabledProperty = DependencyProperty.Register(
            "IsMouseOverHiglightEnabled", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(true));

        public bool IsMouseOverHiglightEnabled
        {
            get { return (bool)GetValue(IsMouseOverHiglightEnabledProperty); }
            set { SetValue(IsMouseOverHiglightEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedOnKeyboardFocusProperty = DependencyProperty.Register(
            "IsHighlightedOnKeyboardFocus", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(true));

        public bool IsHighlightedOnKeyboardFocus
        {
            get { return (bool)GetValue(IsHighlightedOnKeyboardFocusProperty); }
            set { SetValue(IsHighlightedOnKeyboardFocusProperty, value); }
        }


        public static readonly DependencyProperty IsColorShiftProperty = DependencyProperty.Register(
    "IsColorShift", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(true));

        public bool IsColorShift
        {
            get { return (bool)GetValue(IsColorShiftProperty); }
            set { SetValue(IsColorShiftProperty, value); }
        }

        #region Highlight Brushes

        public static readonly DependencyProperty HighlightedBackgroundProperty = DependencyProperty.Register(
           "HighlightedBackground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(null));

        public Brush HighlightedBackground
        {
            get { return (Brush)GetValue(HighlightedBackgroundProperty); }
            set { SetValue(HighlightedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightedForegroundProperty = DependencyProperty.Register(
            "HighlightedForeground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(null));

        public Brush HighlightedForeground
        {
            get { return (Brush)GetValue(HighlightedForegroundProperty); }
            set { SetValue(HighlightedForegroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightedBorderBrushProperty = DependencyProperty.Register(
           "HighlightedBorderBrush", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(null));

        public Brush HighlightedBorderBrush
        {
            get { return (Brush)GetValue(HighlightedBorderBrushProperty); }
            set { SetValue(HighlightedBorderBrushProperty, value); }
        }

        #endregion //End Highlight Brushes


        public static readonly DependencyProperty IsClickedProperty = DependencyProperty.Register(
            "IsClicked", typeof(bool), typeof(BaseColoredButton), new PropertyMetadata(default(bool)));

        public bool IsClicked
        {
            get { return (bool)GetValue(IsClickedProperty); }
            set { SetValue(IsClickedProperty, value); }
        }

        #region Disabled Brushes

        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
           "DisabledBackground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register(
            "DisabledBorderBrush", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush DisabledBorderBrush
        {
            get { return (Brush)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }

        #endregion //End Disabled Brushes

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(BaseColoredButton), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }


        #region Pressed Brushes

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
           "PressedBackground", typeof(Brush), typeof(BaseColoredButton)); // highlighted by default

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(
            "PressedBorderBrush", typeof(Brush), typeof(BaseColoredButton)); // highlighted by default

        public Brush PressedBorderBrush
        {
            get { return (Brush)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
            "PressedForeground", typeof(Brush), typeof(BaseColoredButton)); // highlighted by default

        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        #endregion //End Pressed Brushes

        #region Triggered Colors

        public static readonly DependencyProperty TriggeredBackgroundProperty = DependencyProperty.Register(
           "TriggeredBackground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredBackground
        {
            get { return (Brush)GetValue(TriggeredBackgroundProperty); }
            set { SetValue(TriggeredBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredBorderBrushProperty = DependencyProperty.Register(
            "TriggeredBorderBrush", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredBorderBrush
        {
            get { return (Brush)GetValue(TriggeredBorderBrushProperty); }
            set { SetValue(TriggeredBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty TriggeredForegroundProperty = DependencyProperty.Register(
            "TriggeredForeground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredForeground
        {
            get { return (Brush)GetValue(TriggeredForegroundProperty); }
            set { SetValue(TriggeredForegroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredHighlightedBackgroundProperty = DependencyProperty.Register(
           "TriggeredHighlightedBackground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredHighlightedBackground
        {
            get { return (Brush)GetValue(TriggeredHighlightedBackgroundProperty); }
            set { SetValue(TriggeredHighlightedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredHighlightedBorderBrushProperty = DependencyProperty.Register(
            "TriggeredHighlightedBorderBrush", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredHighlightedBorderBrush
        {
            get { return (Brush)GetValue(TriggeredHighlightedBorderBrushProperty); }
            set { SetValue(TriggeredHighlightedBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty TriggeredHighlightedForegroundProperty = DependencyProperty.Register(
            "TriggeredHighlightedForeground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredHighlightedForeground
        {
            get { return (Brush)GetValue(TriggeredHighlightedForegroundProperty); }
            set { SetValue(TriggeredHighlightedForegroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredPressedBackgroundProperty = DependencyProperty.Register(
           "TriggeredPressedBackground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredPressedBackground
        {
            get { return (Brush)GetValue(TriggeredPressedBackgroundProperty); }
            set { SetValue(TriggeredPressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredPressedBorderBrushProperty = DependencyProperty.Register(
            "TriggeredPressedBorderBrush", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredPressedBorderBrush
        {
            get { return (Brush)GetValue(TriggeredPressedBorderBrushProperty); }
            set { SetValue(TriggeredPressedBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty TriggeredPressedForegroundProperty = DependencyProperty.Register(
            "TriggeredPressedForeground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredPressedForeground
        {
            get { return (Brush)GetValue(TriggeredPressedForegroundProperty); }
            set { SetValue(TriggeredPressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredDisabledBackgroundProperty = DependencyProperty.Register(
   "TriggeredDisabledBackground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredDisabledBackground
        {
            get { return (Brush)GetValue(TriggeredDisabledBackgroundProperty); }
            set { SetValue(TriggeredDisabledBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TriggeredDisabledBorderBrushProperty = DependencyProperty.Register(
            "TriggeredDisabledBorderBrush", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredDisabledBorderBrush
        {
            get { return (Brush)GetValue(TriggeredDisabledBorderBrushProperty); }
            set { SetValue(TriggeredDisabledBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty TriggeredDisabledForegroundProperty = DependencyProperty.Register(
            "TriggeredDisabledForeground", typeof(Brush), typeof(BaseColoredButton), new PropertyMetadata(default(Brush)));

        public Brush TriggeredDisabledForeground
        {
            get { return (Brush)GetValue(TriggeredDisabledForegroundProperty); }
            set { SetValue(TriggeredDisabledForegroundProperty, value); }
        }

        #endregion // End Triggered Colors
    }
}
