using GalaSoft.MvvmLight.CommandWpf;
using Nimedicus.Utils;
using Nimedicus.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nimedicus.Controls
{
    /// <summary>
    /// Логика взаимодействия для WaitDialog.xaml
    /// </summary>
    public partial class WaitDialog : Window, INotifyPropertyChanged
    {
        private const int WaitDialogShowDelay = 500;
        private int? _customWaitDialogShowDelay;

        public bool DialogShouldBeShown;
        public bool ForceShowDialog;

        public new void ShowDialog()
        {
            new Thread(() =>
            {
                if (!ForceShowDialog)
                {
                    var delay = _customWaitDialogShowDelay ?? WaitDialogShowDelay;
                    Thread.Sleep(delay);
                }
                try
                {
                    ThreadHelper.ExecuteInMainDispatcher(() =>
                    {
                        try
                        {
                            Mouse.OverrideCursor = _previousMouseCursor;
                            if (DialogShouldBeShown)
                            {
                                base.ShowDialog();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });

                }
                catch (Exception)
                {
                }
            }).Start();
        }

        public new void Close()
        {
            //lock (dialogShouldBeShownLockObject)
            //{
            allowClosing = true;
            DialogShouldBeShown = false;
            base.Close();
            //}
        }

        #region Static Members

        public static bool CancelationPending;
        public static WaitDialog LastWaitDialog = null;
        private static List<WaitDialog> m_WaitDialogs = new List<WaitDialog>();
        private static Cursor _previousMouseCursor = Cursors.Arrow;
        //private static object dialogShouldBeShownLockObject = new object();

        /// <summary>
        /// Shows WaitDialog without background work routine, so it should be closed manualy
        /// </summary>
        /// <param name="mainText">Text shown in center of dialog to describe it's actions, By defult "Please Wait..." !! even if null is passed !!</param>
        /// <param name="cancelButtonText">Text for cancel button, By default cancel if null is passed</param>
        /// <param name="buttonCancelRoutine">Action which should be executed on canceiling operation  !! if null it's ignored and not executed !!</param>
        /// <param name="canCancel">Param which shows wheter cancel button should be visible, By default not visible, visible despite bool param if buttonCancelRoutine != null</param>
        /// <param name="forceShow"> </param>
        /// <param name="owner">Owner window for dialog, By default Application.Current.MainWindow, !! even if null is passed !!</param>
        public static void ShowDialogStatic(string mainText = null, string cancelButtonText = null, Action buttonCancelRoutine = null, bool canCancel = false, bool forceShow = false, Window owner = null, int? customWaitDialogShowDelay = null)
        {
            if (Mouse.OverrideCursor != Cursors.Wait)
            {
                _previousMouseCursor = Mouse.OverrideCursor;
            }
            //Mouse.OverrideCursor = Cursors.Wait;

            if (owner != null && !owner.IsLoaded)
            {
                //owner.Loaded += (sender, args) => Owner = owner;
                owner = null;
            }

            LastWaitDialog = new WaitDialog(owner,
                mainText,
                cancelButtonText,
                canCancel,
                buttonCancelRoutine,
                customWaitDialogShowDelay);

            m_WaitDialogs.Add(LastWaitDialog);

            LastWaitDialog.ForceShowDialog = forceShow;

            _prevOwner = owner;
            _prevButtonCancelRoutine = buttonCancelRoutine;
            _prevMainText = mainText;
            _prevCancelButtonText = cancelButtonText;
            _prevCanCancel = canCancel;
            _prevCustomWaitDialogShowDelay = customWaitDialogShowDelay;

            LastWaitDialog.ShowDialog();
        }

        /// <summary>
        /// Closes WaitDialog if it's present
        /// </summary>
        public static void TryCloseWaitDialog()
        {
            var waitDialog = m_WaitDialogs.LastOrDefault();
            if (waitDialog != null)
            {
                m_WaitDialogs.Remove(waitDialog);
                waitDialog.Dispatcher.Invoke(new ThreadStart(waitDialog.Close));
                if (m_WaitDialogs.Count == 0)
                {
                    LastWaitDialog = null;
                }
            }
        }

        public static bool WaitDialogExist()
        {
            return m_WaitDialogs.Count > 0;
        }

        /// <summary>
        /// Closes WaitDialog if it's present
        /// </summary>
        public static void ShowPreviousWaitDialog()
        {
            LastWaitDialog = new WaitDialog(_prevOwner,
                _prevMainText,
                _prevCancelButtonText,
                _prevCanCancel,
                _prevButtonCancelRoutine,
                _prevCustomWaitDialogShowDelay);
            m_WaitDialogs.Add(LastWaitDialog);
            LastWaitDialog.ForceShowDialog = true;

            LastWaitDialog.ShowDialog();
        }

        #endregion

        private Action m_buttonCancelRoutine = null;

        #region DataContextProperties

        private Visibility _cancelButtonVisibility;
        public Visibility CancelButtonVisibility
        {
            get { return _cancelButtonVisibility; }
            set
            {
                if (_cancelButtonVisibility == value)
                    return;
                _cancelButtonVisibility = value;
                OnPropertyChanged("CancelButtonVisibility");
            }
        }

        private string _waitText;
        public string WaitText
        {
            get
            {
#if ABLITE
                return _waitText?.ToUpper();
#endif
                return _waitText;
            }
            set
            {
                if (_waitText == value)
                    return;
                _waitText = value;
                OnPropertyChanged("WaitText");
            }
        }

        private string _cancelButtonText;
        public string CancelButtonText
        {
            get { return _cancelButtonText; }
            set
            {
                if (_cancelButtonText == value)
                    return;
                _cancelButtonText = value;
                OnPropertyChanged("CancelButtonText");
            }

        }

        #endregion

        #region Private Members

        private WaitDialog(Window owner,
            string mainText,
            string cancelButtonText,
            bool canCancel,
            Action buttonCancelRoutine,
            int? customWaitDialogShowDelay)
        {
            WaitText = "Очікуйте...";

            DataContext = this;

            DialogShouldBeShown = true;
            CancelationPending = false;

            if (owner == null && Application.Current != null)
            {
                owner = Application.Current.MainWindow;
            }

            if (mainText == null)
            {
                mainText = "Зачекайте, будь ласка...";
            }

            if (cancelButtonText == null)
            {
                cancelButtonText = "Отмена";
            }

            if (buttonCancelRoutine != null)
            {
                canCancel = true;
            }

            InitializeComponent();

            Loaded += WaitDialog_Loaded;
            ContentRendered += (s, e) => { InvalidateMeasure(); InvalidateVisual(); };


            if (!canCancel)
            {
                CancelButtonVisibility = Visibility.Collapsed;
            }

            Owner = owner;

            WaitText = mainText;
            CancelButtonText = cancelButtonText;
            m_buttonCancelRoutine = buttonCancelRoutine;
            DialogShouldBeShown = true;
            _customWaitDialogShowDelay = customWaitDialogShowDelay;
        }

        #region DisableDragWindow && CloseButton

        void WaitDialog_Loaded(object sender, EventArgs e)
        {
            var helper = new WindowInteropHelper(this);
            if (helper.Handle.ToInt32() > 0)
            {
                var source = HwndSource.FromHwnd(helper.Handle);
                source.AddHook(WndProc);
            }
        }

        /// <summary>
        /// This field should be set to True if dialog is allowed to close(set to true before programmaticali close it)
        /// Wait Dialog was tweaked by Windows System commands to prevent closing by alt+f4 etc. 
        /// </summary>
        private bool allowClosing = false;

        private static Window _prevOwner;
        private static Action _prevButtonCancelRoutine;
        private static string _prevMainText;
        private static string _prevCancelButtonText;
        private static bool _prevCanCancel;
        private static int? _prevCustomWaitDialogShowDelay;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);


        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;

        private const uint SC_CLOSE = 0xF060;

        private const int WM_SHOWWINDOW = 0x00000018;
        private const int WM_CLOSE = 0x10;

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
                case WM_SHOWWINDOW:
                    {
                        IntPtr hMenu = GetSystemMenu(hwnd, false);
                        if (hMenu != IntPtr.Zero)
                        {
                            EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                        }
                        InvalidateMeasure();
                        InvalidateVisual();

                    }
                    break;
                case WM_CLOSE:
                    if (!allowClosing)
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

        #endregion

        #region CancelCommand

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        private void Cancel()
        {
            if (m_buttonCancelRoutine != null)
                m_buttonCancelRoutine();

            CancelationPending = true;
            Close();
        }


        private RelayCommand _cancelCommand;

        #endregion //End CancelCommand

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
