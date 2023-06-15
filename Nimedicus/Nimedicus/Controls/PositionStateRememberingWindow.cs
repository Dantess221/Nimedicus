using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media;
using Point = System.Windows.Point;
using Microsoft.Win32;

namespace Nimedicus.Controls
{
    public abstract class PositionStateRememberingWindow : Window
    {
        #region ChildWindow Managment

        public bool IsAnyDialogOpened
        {
            get { return ChildWindows != null && ChildWindows.Any(); }
        }

        public List<Window> ChildWindows;

        public void AddChildWindow(Window window)
        {
            if (ChildWindows == null)
            {
                ChildWindows = new List<Window>();
            }

            ChildWindows.Add(window);
        }

        public void RemoveChildWindow(Window window)
        {
            if (ChildWindows.Contains(window))
            {
                ChildWindows.Remove(window);
            }
        }
        #endregion

        public double Right => Left + Width;
        public double Bottom => Top + Height;

        //public event DpiChangedEventHandler DpiChangedEvent;

        public virtual string RegistryPrefix => "";

        public PositionStateRememberingWindow()
        {

            Left = (int)Registry.GetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\" + "Config\\", $"{RegistryPrefix}WindowLocationX", -99999);
            Top = (int)Registry.GetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\" + "Config\\", $"{RegistryPrefix}WindowLocationY", -99999);

            var state = (WindowState)Registry.GetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\" + "Config\\", "WindowState", 0);

            if (state == WindowState.Minimized)
            {
                state = WindowState.Normal;
            }

            WindowState = state;

            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiX = (int)dpiXProperty.GetValue(null, null);

            if (Top == -99999 && Left == -99999)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (dpiX >= 288)
                {
                    WindowState = WindowState.Maximized;
                }
            }
            else
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
            }

            Closing += OnClosing;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FixMinHeightWidthAccordingToDPIAndResolution();
            int width = (int)Registry.GetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\" + "Config\\", $"{RegistryPrefix}WindowSizeWith", 0);
            int height = (int)Registry.GetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\" + "Config\\", $"{RegistryPrefix}WindowSizeHeight", 0);
            if (width != 0)
            {
                Width = width;
            }

            if (height != 0)
            {
                Height = height;
            }

            SizeToFit();
            MoveIntoView();
        }

        protected virtual void FixMinHeightWidthAccordingToDPIAndResolution()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiCoeeficient = (double)96 / dpiX;

            if (!double.IsNaN(MinWidth))
            {
                MinWidth = Math.Min(MinWidth, Screen.PrimaryScreen.WorkingArea.Width * dpiCoeeficient);
            }

            if (!double.IsNaN(MinHeight))
            {
                MinHeight = Math.Min(MinHeight, Screen.PrimaryScreen.WorkingArea.Height * dpiCoeeficient);
            }
        }

        private void OnClosing(object sender, EventArgs eventArgs)
        {
            if (WindowState == WindowState.Normal)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\"+ "Config\\", $"{RegistryPrefix}WindowLocationY", (int)Top);
                Registry.SetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\"+ "Config\\", $"{RegistryPrefix}WindowLocationX", (int)Left);
                Registry.SetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\"+ "Config\\", $"{RegistryPrefix}WindowSizeWith", (int)Width);
                Registry.SetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\"+ "Config\\", $"{RegistryPrefix}WindowSizeHeight", (int)Height);
            }

            Registry.SetValue("HKEY_CURRENT_USER\\" + "Software\\Nimedicus\\" + "Config\\", $"{RegistryPrefix}WindowState", (int)WindowState);
        }

        public void SizeToFit()
        {
            if (Height > SystemParameters.VirtualScreenHeight)
            {
                Height = SystemParameters.VirtualScreenHeight;
            }

            if (Width > SystemParameters.VirtualScreenWidth)
            {
                Width = SystemParameters.VirtualScreenWidth;
            }
        }

        public void MoveIntoView()
        {
            var workSpace = SystemParameters.WorkArea;
            if (Top < workSpace.Top)
            {
                Top = workSpace.Top;
            }

            if (!IsFullyVisible())
            {
                Top = 0;
                Left = 0;
            }

            if (Top + Height / 2 >
                 SystemParameters.VirtualScreenHeight)
            {
                Top = SystemParameters.VirtualScreenHeight - Height;
            }

            if (Left + Width / 2 > SystemParameters.VirtualScreenWidth)
            {
                Left = SystemParameters.VirtualScreenWidth - Width;
            }
        }

        bool IsPointVisibleOnAScreen(Point p)
        {
            var converter = new ScreenBoundsConverter(this);

            foreach (var screen in Screen.AllScreens)
            {
                var bounds = converter.ConvertBounds(screen.Bounds);

                if (p.X < bounds.Right && p.X > bounds.Left && p.Y > bounds.Top && p.Y < bounds.Bottom)
                    return true;
            }

            return false;
        }

        bool IsFullyVisible()
        {
            return IsPointVisibleOnAScreen(new Point(Left, Top)) &&
                IsPointVisibleOnAScreen(new Point(Right, Top)) &&
                IsPointVisibleOnAScreen(new Point(Left, Bottom)) &&
                IsPointVisibleOnAScreen(new Point(Right, Bottom));
        }

        public class ScreenBoundsConverter
        {
            private Matrix _transform;

            public ScreenBoundsConverter(Visual visual)
            {
                _transform =
                    PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            }

            public Rect ConvertBounds(Rectangle bounds)
            {
                var result = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                result.Transform(_transform);
                return result;
            }
        }
    }
}
