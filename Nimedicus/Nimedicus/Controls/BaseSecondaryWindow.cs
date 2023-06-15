using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Controls;

namespace Nimedicus.Controls
{
    public class BaseSecondaryWindow : Window, INotifyPropertyChanged
    {
        public MessageBoxResult WindowResult;
        private MessageBoxResult _defaultResult;
        protected bool IsEscCloseEnabled;
        protected bool IsEnterOkEnabled;
        protected bool IsDragEnabled;

        #region Constructors

        protected BaseSecondaryWindow()
        {
            Initialize(MessageBoxResult.None, true, true, true);
        }

        protected BaseSecondaryWindow(MessageBoxResult defaultMessageBoxResult)
        {
            Initialize(defaultMessageBoxResult, true, true, true);
        }

        protected BaseSecondaryWindow(MessageBoxResult defaultMessageBoxResult, bool isIsEscCloseEnabled)
        {
            Initialize(defaultMessageBoxResult, isIsEscCloseEnabled, true, true);
        }

        protected BaseSecondaryWindow(MessageBoxResult defaultMessageBoxResult, bool isIsEscCloseEnabled,
            bool isEnterOkEnabled)
        {
            Initialize(defaultMessageBoxResult, isIsEscCloseEnabled, isEnterOkEnabled, true);
        }

        protected BaseSecondaryWindow(MessageBoxResult defaultMessageBoxResult, bool isIsEscCloseEnabled,
            bool isEnterOkEnabled, bool isDragEnabled)
        {
            Initialize(defaultMessageBoxResult, isIsEscCloseEnabled, isEnterOkEnabled, isDragEnabled);
        }

        #endregion// End Constructors

        public new void ShowDialog()
        {
            WaitDialog.TryCloseWaitDialog();
            base.ShowDialog();
        }

        private void Initialize(MessageBoxResult defaultMessageBoxResult, bool isIsEscCloseEnabled,
            bool isEnterOkEnabled, bool isDragEnabled)
        {
            WindowResult = _defaultResult = defaultMessageBoxResult;
            IsEscCloseEnabled = isIsEscCloseEnabled;
            IsEnterOkEnabled = isEnterOkEnabled;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            IsDragEnabled = isDragEnabled;

            if (!IsDragEnabled)
            {
                // Currently it only disables window dragging
                SourceInitialized += BaseSecondaryWindow_SourceInitialized;
            }

            PreviewKeyDown += WindowKeyDown;
            ContentRendered += (s, e) => { InvalidateMeasure(); InvalidateVisual(); };
            MouseDown += Header_MouseLeftButtonDown;
            Loaded += (o, e) => { };
            try
            {
                Owner = Application.Current.MainWindow;
            }
            catch (InvalidOperationException)
            {
                try
                {
                    Application.Current.MainWindow.ContentRendered += (sender, args) => Owner = Application.Current.MainWindow;
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        protected virtual void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && IsEscCloseEnabled)
            {
                e.Handled = true;
                _defaultResult = MessageBoxResult.Cancel;
                Close();
            }
            if (e.Key == Key.Enter)
            {
                var button = Keyboard.FocusedElement as Button;
                if (button != null)
                {
                    typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[0]);
                    e.Handled = true;
                }
                else if (IsEnterOkEnabled)
                {
                    var textBox = Keyboard.FocusedElement as TextBox;
                    if (textBox != null)
                    {
                        e.Handled = true;
                        OkButton_Click(null, new RoutedEventArgs());
                    }
                }
            }
        }

        #region Disable Window Draging

        void BaseSecondaryWindow_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;

                default:
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion

        protected virtual void OkButton_Click(object sender, RoutedEventArgs e)
        {
            WindowResult = MessageBoxResult.OK;
            Close();
        }

        protected virtual void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowResult = MessageBoxResult.Cancel;
            Close();
        }

        protected virtual void YesButton_Click(object sender, RoutedEventArgs e)
        {
            WindowResult = MessageBoxResult.Yes;
            Close();
        }

        protected virtual void NoButton_Click(object sender, RoutedEventArgs e)
        {
            WindowResult = MessageBoxResult.No;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
