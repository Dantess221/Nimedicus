using Nimedicus.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Controls;
using Nimedicus.Utils;
using System.Windows.Threading;

namespace Nimedicus.Controls.DTMessageBox
{
    public class DTMessageBox : BaseSecondaryWindow
    {

        #region Properties

        public MessageBoxButton MessageBoxButtons { get; set; }

        public MessageBoxImage MessageBoxImage { get; set; }

        private ImageSource _errorIcon;
        public ImageSource ErrorIcon
        {
            get { return _errorIcon; }
            set
            {
                if (_errorIcon == value)
                    return;
                _errorIcon = value;
                OnPropertyChanged("ErrorIcon");
            }

        }

        private Drawing _errorIconSvg;
        public Drawing ErrorIconSvg
        {
            get { return _errorIconSvg; }
            set
            {
                if (_errorIconSvg == value)
                    return;
                _errorIconSvg = value;
                OnPropertyChanged("ErrorIconSvg");
            }
        }

        private string _messageTitle;
        public string MessageTitle
        {
            get { return _messageTitle; }
            set
            {
                if (_messageTitle == value)
                    return;
                _messageTitle = value;
                OnPropertyChanged("MessageTitle");
            }

        }

        private string _messageText;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                if (value == _messageText)
                {
                    return;
                }
                _messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        private string _okButtonText = "OK";
        public string OkButtonText
        {
            get { return _okButtonText; }
            set
            {
                if (_okButtonText == value)
                    return;
                _okButtonText = value;
                OnPropertyChanged("OkButtonText");
            }
        }

        private string _cancelButtonText = "Cancel";
        public string CancelButtonText
        {
            get { return _cancelButtonText; }
            set
            {
                if (_cancelButtonText == value)
                    return;
                _cancelButtonText = value;
                OnPropertyChanged(nameof(CancelButtonText));
            }
        }
        private string _yesButtonText = "Yes";
        public string YesButtonText
        {
            get { return _yesButtonText; }
            set
            {
                if (_yesButtonText == value)
                    return;
                _yesButtonText = value;
                OnPropertyChanged("YesButtonText");
            }
        }
        private string _noButtonText = "No";
        public string NoButtonText
        {
            get { return _noButtonText; }
            set
            {
                if (_noButtonText == value)
                    return;
                _noButtonText = value;
                OnPropertyChanged("NoButtonText");
            }
        }

        private bool _okIsUACButton;
        public bool OkIsUACButton
        {
            get { return _okIsUACButton; }
            set
            {
                if (_okIsUACButton == value)
                    return;
                _okIsUACButton = value;
                OnPropertyChanged(nameof(OkIsUACButton));
            }
        }

        private Visibility _yesButtonVisibility;
        public Visibility YesButtonVisibility
        {
            get
            {
                return _yesButtonVisibility;
            }
            set
            {
                if (_yesButtonVisibility == value)
                    return;
                _yesButtonVisibility = value;
                OnPropertyChanged("YesButtonVisibility");
            }

        }

        private Visibility _noButtonVisibility;
        public Visibility NoButtonVisibility
        {
            get { return _noButtonVisibility; }
            set
            {
                if (_noButtonVisibility == value)
                    return;
                _noButtonVisibility = value;
                OnPropertyChanged("NoButtonVisibility");
            }

        }

        private Visibility _okButtonVisibility;
        public Visibility OkButtonVisibility
        {
            get { return _okButtonVisibility; }
            set
            {
                if (_okButtonVisibility == value)
                    return;
                _okButtonVisibility = value;
                OnPropertyChanged("OkButtonVisibility");
            }

        }

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

        public GridLength Col1Width
        {
            get
            {
                if (MessageBoxButtons == MessageBoxButton.YesNo)
                {
                    return new GridLength(1, GridUnitType.Star);
                }

                return GridLength.Auto;
            }
        }

        public GridLength Col2Width
        {
            get
            {
                if (MessageBoxButtons == MessageBoxButton.YesNo || MessageBoxButtons == MessageBoxButton.YesNoCancel)
                {
                    return new GridLength(1, GridUnitType.Star);
                }

                return GridLength.Auto;
            }
        }

        public GridLength Col3Width
        {
            get
            {
                if (MessageBoxButtons == MessageBoxButton.OK || MessageBoxButtons == MessageBoxButton.OKCancel)
                {
                    return new GridLength(1, GridUnitType.Star);
                }

                return GridLength.Auto;
            }
        }

        public GridLength Col4Width
        {
            get
            {
                if (MessageBoxButtons == MessageBoxButton.OKCancel)
                {
                    return new GridLength(1, GridUnitType.Star);
                }

                return GridLength.Auto;
            }
        }


        #endregion

        #region Commands

        #region OK

        public ICommand OKCommand
        {
            get { return _oK ?? (_oK = new RelayCommand(OK)); }
        }

        private void OK()
        {
            WindowResult = MessageBoxResult.OK;
            Close();
        }

        private RelayCommand _oK;

        #endregion //End OK

        #region Cancel

        public ICommand CancelCommand
        {
            get { return _cancel ?? (_cancel = new RelayCommand(Cancel)); }
        }

        private void Cancel()
        {
            WindowResult = MessageBoxResult.Cancel;
            Close();
        }

        private RelayCommand _cancel;

        #endregion //End Cancel

        #region Yes

        public ICommand YesCommand
        {
            get { return _yes ?? (_yes = new RelayCommand(Yes)); }
        }

        private void Yes()
        {
            WindowResult = MessageBoxResult.Yes;
            Close();
        }

        private RelayCommand _yes;

        #endregion //End Yes

        #region No

        public ICommand NoCommand
        {
            get { return _no ?? (_no = new RelayCommand(No)); }
        }

        private void No()
        {
            WindowResult = MessageBoxResult.No;
            Close();
        }

        private RelayCommand _no;

        #endregion //End No 

        #endregion

        #region ControlGetters

        private Button OkButton
        {
            get { return Template.FindName("OkButton", this) as Button; }
        }

        private Button YesButton
        {
            get { return Template.FindName("YesButton", this) as Button; }
        }

        #endregion

        protected DTMessageBox(
            Window owner, string text, string titleText, MessageBoxButton button, MessageBoxImage icon, string okText, bool okIsUacButton = false, double timeout = 0, MessageBoxResult defResult = MessageBoxResult.None,
            string cancelText = null, string yesText = null, string noText = null)
            : base(defResult)
        {
            //InitializeComponent();
            DataContext = this;
            //MessageBoxText.Text = text;
            MessageText = text;
            //MessageTitle.Text = title;
            OkIsUACButton = okIsUacButton;

            if (Owner == null || Owner.Visibility == Visibility.Hidden)
            {
                ShowInTaskbar = true;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            Owner = owner;
            MessageBoxButtons = button;
            MessageBoxImage = icon;

            #region Set Default focus for buttons upon Loaded

            Loaded += (sender, args) =>
            {
                switch (button)
                {
                    case MessageBoxButton.OK:
                    case MessageBoxButton.OKCancel:
                        OkButton?.Focus();
                        Keyboard.Focus(OkButton);
                        break;

                    case MessageBoxButton.YesNo:
                    case MessageBoxButton.YesNoCancel:
                        YesButton?.Focus();
                        Keyboard.Focus(YesButton);
                        break;
                }

            };

            #endregion // Set Default focus for buttons upon Loaded

            #region Set Buttons Visibility

            switch (button)
            {
                case MessageBoxButton.OK:
                    YesButtonVisibility = Visibility.Collapsed;
                    NoButtonVisibility = Visibility.Collapsed;
                    CancelButtonVisibility = Visibility.Collapsed;
                    if (!string.IsNullOrEmpty(okText))
                    {
                        OkButtonText = okText;
                    }
                    else
                    {
                        try
                        {
                            OkButtonText = "OK";
                        }
                        catch (Exception) { }
                        if (string.IsNullOrEmpty(OkButtonText))
                        {
                            OkButtonText = "OK";
                        }
                    }
                    break;

                case MessageBoxButton.OKCancel:
                    YesButtonVisibility = Visibility.Collapsed;
                    NoButtonVisibility = Visibility.Collapsed;
                    if (!string.IsNullOrEmpty(okText))
                    {
                        OkButtonText = okText;
                    }
                    else
                    {
                        try
                        {
                            OkButtonText = "OK";
                        }
                        catch (Exception) { }

                        if (string.IsNullOrEmpty(OkButtonText))
                        {
                            OkButtonText = "OK";
                        }
                    }
                    if (!string.IsNullOrEmpty(cancelText))
                    {
                        CancelButtonText = cancelText;
                    }
                    else
                    {
                        try
                        {
                            CancelButtonText = "Cancel";
                        }
                        catch (Exception) { }

                        if (string.IsNullOrEmpty(CancelButtonText))
                        {
                            CancelButtonText = "Cancel";
                        }
                    }
                    break;

                case MessageBoxButton.YesNo:
                    OkButtonVisibility = Visibility.Collapsed;
                    CancelButtonVisibility = Visibility.Collapsed;
                    if (!string.IsNullOrEmpty(yesText))
                    {
                        YesButtonText = yesText;
                    }
                    else
                    {
                        try
                        {
                            YesButtonText = "Yes";
                        }
                        catch (Exception) { }

                        if (string.IsNullOrEmpty(YesButtonText))
                        {
                            YesButtonText = "Yes";
                        }
                    }
                    if (!string.IsNullOrEmpty(noText))
                    {
                        NoButtonText = noText;
                    }
                    else
                    {
                        try
                        {
                            NoButtonText = "No";
                        }
                        catch (Exception) { }

                        if (string.IsNullOrEmpty(NoButtonText))
                        {
                            NoButtonText = "No";
                        }
                    }
                    break;

                case MessageBoxButton.YesNoCancel:
                    OkButtonVisibility = Visibility.Collapsed;
                    if (!string.IsNullOrEmpty(yesText))
                    {
                        YesButtonText = yesText;
                    }
                    else
                    {
                        try
                        {
                            YesButtonText = "Yes";
                        }
                        catch (Exception) { }

                        if (string.IsNullOrEmpty(YesButtonText))
                        {
                            YesButtonText = "Yes";
                        }
                    }
                    if (!string.IsNullOrEmpty(noText))
                    {
                        NoButtonText = noText;
                    }
                    else
                    {
                        try
                        {
                            NoButtonText = "No";
                        }
                        catch (Exception) { }
                        if (string.IsNullOrEmpty(NoButtonText))
                        {
                            NoButtonText = "No";
                        }
                    }
                    if (!string.IsNullOrEmpty(cancelText))
                    {
                        CancelButtonText = cancelText;
                    }
                    else
                    {
                        try
                        {
                            CancelButtonText = "Cancel";
                        }
                        catch (Exception) { }
                        if (string.IsNullOrEmpty(CancelButtonText))
                        {
                            CancelButtonText = "Cancel";
                        }
                    }
                    break;

                default:
                    break;
            }

            #endregion // Set buttons Visibility

            #region Set Current MessageBox Icon

            if (MessageBoxImage == MessageBoxImage.None)
            {
                switch (button)
                {
                    case MessageBoxButton.OK:
                        MessageBoxImage = MessageBoxImage.Warning;
                        break;

                    case MessageBoxButton.OKCancel:
                    case MessageBoxButton.YesNo:
                    case MessageBoxButton.YesNoCancel:
                        MessageBoxImage = MessageBoxImage.Question;
                        break;
                }
            }

            switch (MessageBoxImage)
            {
                case MessageBoxImage.Information:
                    ErrorIcon = Application.Current.TryFindResource("IcoInformation") as ImageSource;
                    ErrorIconSvg = Application.Current.TryFindResource("Information") as Drawing;
                    break;

                case MessageBoxImage.Error:
                    ErrorIcon = Application.Current.TryFindResource("IcoError") as ImageSource;
                    ErrorIconSvg = Application.Current.TryFindResource("Error") as Drawing;
                    break;

                case MessageBoxImage.Warning:
                    ErrorIcon = Application.Current.TryFindResource("IcoWarning") as ImageSource;
                    ErrorIconSvg = Application.Current.TryFindResource("Warning") as Drawing;
                    break;

                case MessageBoxImage.Question:
                    ErrorIcon = Application.Current.TryFindResource("IcoQuestion") as ImageSource;
                    ErrorIconSvg = Application.Current.TryFindResource("Question") as Drawing;
                    break;
            }

            #endregion // Set current MessageBox Icon

            string dtMessageBoxTitle = string.Empty;

            if (string.IsNullOrEmpty(titleText))
            {
                switch (MessageBoxImage)
                {
                    case MessageBoxImage.Information:
                        dtMessageBoxTitle = "Information";
                        break;

                    case MessageBoxImage.Error:
                        dtMessageBoxTitle = "Error";
                        break;

                    case MessageBoxImage.Warning:
                        dtMessageBoxTitle = "Information";
                        break;

                    case MessageBoxImage.Question:
                        dtMessageBoxTitle = "Information";
                        break;
                }
            }
            else
            {
                dtMessageBoxTitle = titleText;
            }

            MessageTitle = dtMessageBoxTitle;

            #region Set Timeout for closing

            if (timeout > 0)
            {
                StartCloseTimer(timeout);
            }

            #endregion

            if (Owner is PositionStateRememberingWindow)
            {
                ((PositionStateRememberingWindow)Owner).AddChildWindow(this);
            }

            if (titleText == null)
            {
                Title = "Nimedicus";
            }
        }

        #region ShowAsync

        private const int _asyncWaitStep = 100;
        private const int _asyncWaitTimeout = 30000;

        /// <summary>
        /// This method should be used in async mode and without await(Await only if you are sure that it will not halt MainWindow initilization in any way).
        /// It will wait for App.Current.MainWindow to initilize if it is not initialized
        /// so that it could properly set MainWindow as Owner to self
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <param name="okText"></param>
        /// <returns></returns>
        public static async Task ShowAsync(Window owner, string text, string title, MessageBoxButton button, MessageBoxImage icon, string okText)
        {
            if (!owner.IsLoaded)
            {
                int alreadyWait = 0;
                await Task.Run(async () =>
                {
                    bool ownerIsLoaded = false;
                    ThreadHelper.ExecuteInMainDispatcher(() => ownerIsLoaded = owner.IsLoaded);
                    while (!ownerIsLoaded && alreadyWait < _asyncWaitTimeout)
                    {
                        await Task.Delay(_asyncWaitStep);
                        alreadyWait += _asyncWaitStep;
                    }
                });
            }

            if (WaitDialog.LastWaitDialog != null)
            {
                WaitDialog.LastWaitDialog.DialogShouldBeShown = false;
            }

            var dtMessageBox = new DTMessageBox(owner, text, title, button, icon, okText);
            dtMessageBox.ShowDialog();

            if (WaitDialog.LastWaitDialog != null)
            {
                WaitDialog.LastWaitDialog.DialogShouldBeShown = true;
            }
        }

        public static async Task ShowAsync(Window owner, string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            await ShowAsync(owner, text, title, button, icon, "");
        }

        public static async Task ShowAsync(Window owner, string text, string title, MessageBoxButton button)
        {
            await ShowAsync(owner, text, title, button, MessageBoxImage.Information);
        }

        public static async Task ShowAsync(Window owner, string text, MessageBoxButton button, MessageBoxImage icon)
        {
            await ShowAsync(owner, text, null, button, icon);
        }

        public static async Task ShowAsync(string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            await ShowAsync(Application.Current.MainWindow, text, title, button, icon);
        }

        public static async Task ShowAsync(string text, string title, MessageBoxButton button)
        {
            await ShowAsync(Application.Current.MainWindow, text, title, button, MessageBoxImage.Information);
        }

        public static async Task ShowAsync(string text, MessageBoxButton button, MessageBoxImage icon, string btnOkText = null)
        {
            await ShowAsync(Application.Current.MainWindow, text, null, button, icon, btnOkText);
        }

        public static async Task ShowAsync(string text, MessageBoxButton button)
        {
            await ShowAsync(Application.Current.MainWindow, text, null, button, MessageBoxImage.Information);
        }
        public static async Task ShowAsync(string text)
        {
            await ShowAsync(Application.Current.MainWindow, text, null, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion // End ShowAsync

        public static MessageBoxResult Show(Window owner, string text, string title, MessageBoxButton button, MessageBoxImage icon, string okText, bool btnOkIsUac = false, double timeout = 0, MessageBoxResult defResult = MessageBoxResult.None, string cancelText = null, string yesText = null, string noText = null)
        {
            if (WaitDialog.LastWaitDialog != null)
            {
                WaitDialog.LastWaitDialog.DialogShouldBeShown = false;
            }

            var dtMessageBox = new DTMessageBox(owner, text, title, button, icon, okText, btnOkIsUac, timeout, defResult, cancelText, yesText, noText);
            dtMessageBox.ShowDialog();

            if (WaitDialog.LastWaitDialog != null)
            {
                WaitDialog.LastWaitDialog.DialogShouldBeShown = true;
            }

            return dtMessageBox.WindowResult;
        }

        public static MessageBoxResult Show(Window owner, string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(owner, text, title, button, icon, "");
        }

        public static MessageBoxResult Show(Window owner, string text, string title, MessageBoxButton button)
        {
            return Show(owner, text, title, button, MessageBoxImage.Information);
        }

        public static MessageBoxResult Show(Window owner, string text, MessageBoxButton button, MessageBoxImage icon, string btnOkText = null)
        {
            return Show(owner, text, null, button, icon, btnOkText);
        }

        public static MessageBoxResult Show(string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(Application.Current.MainWindow, text, title, button, icon);
        }

        public static MessageBoxResult Show(string text, string title, MessageBoxButton button)
        {
            return Show(Application.Current.MainWindow, text, title, button, MessageBoxImage.Information);
        }

        public static MessageBoxResult Show(string text, string title, MessageBoxButton button, MessageBoxImage icon, string btnOkText = null, bool btnOkIsUac = false)
        {
            return Show(Application.Current.MainWindow, text, title, button, icon, btnOkText, btnOkIsUac);
        }

        public static MessageBoxResult Show(string text, MessageBoxButton button, MessageBoxImage icon, string btnOkText = null, bool btnOkIsUac = false, MessageBoxResult defResult = MessageBoxResult.None)
        {
            return Show(Application.Current.MainWindow, text, null, button, icon, btnOkText, btnOkIsUac, 0, defResult);
        }

        public static MessageBoxResult Show(string text, MessageBoxButton button)
        {
            return Show(Application.Current.MainWindow, text, null, button, MessageBoxImage.Information);
        }

        public static MessageBoxResult Show(string text)
        {
            return Show(Application.Current.MainWindow, text, null, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowError(string text)
        {
            return Show(Application.Current.MainWindow, text, null, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult Show(string text, MessageBoxImage icon, string title, string okText, string cancelText)
        {
            return Show(null, text, title, MessageBoxButton.OKCancel, icon, okText, cancelText: cancelText);
        }

        public static string GetLinkText(string link, string linkName)
        {
            return string.Format("<a href=\"{0}\">{1}</a>", link, linkName);
        }

        public static MessageBoxResult ShowNonBlocking(Window owner, string text, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            if (WaitDialog.LastWaitDialog != null)
            {
                WaitDialog.LastWaitDialog.DialogShouldBeShown = false;
            }

            var dtMessageBox = new DTMessageBox(owner, text, title, button, icon, "");
            dtMessageBox.Show();

            if (WaitDialog.LastWaitDialog != null)
            {
                WaitDialog.LastWaitDialog.DialogShouldBeShown = true;
            }

            return dtMessageBox.WindowResult;
        }

        public static MessageBoxResult ShowNonBlocking(string text, MessageBoxButton button, MessageBoxImage icon)
        {
            return ShowNonBlocking(Application.Current.MainWindow, text, null, button, icon);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Owner is PositionStateRememberingWindow)
            {
                ((PositionStateRememberingWindow)Owner).RemoveChildWindow(this);
            }

            if (WindowResult == MessageBoxResult.None)
            {
                if (MessageBoxButtons == MessageBoxButton.YesNoCancel ||
                    MessageBoxButtons == MessageBoxButton.OKCancel)
                {
                    WindowResult = MessageBoxResult.Cancel;
                }
                else if (MessageBoxButtons == MessageBoxButton.YesNo)
                {
                    WindowResult = MessageBoxResult.No;
                }
                else
                {
                    WindowResult = MessageBoxResult.None;
                }
            }
        }

        private void StartCloseTimer(double timeout)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(timeout);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            Close();
        }
    }
}
