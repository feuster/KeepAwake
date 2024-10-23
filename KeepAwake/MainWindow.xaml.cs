using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using KeepAwake.Assets.Translation;
using System.Reflection;

namespace KeepAwake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ObservableObject]
    public partial class MainWindow : Window
    {
        #region Constants and obsevable properties
        #pragma warning disable S2223, S1104, CS8602, CA2211
        private const string Color1 = "#01245e";
        private const string Color2 = "#DCE6FF";
        private const string Color3 = "#01bfff";
        private const string Transparent = "transparent";
        private static readonly string AppVersion = ($"V{Assembly.GetEntryAssembly().GetName().Version.Major}.{Assembly.GetEntryAssembly().GetName().Version.MinorRevision}") ?? "n/a";

        [ObservableProperty]
        public static string _AppName = "KeepAwake";

        [ObservableProperty]
        public static string _AppLanguage = "en-us";

        [ObservableProperty]
        private static string _TitleText = "KeepAwake";

        [ObservableProperty]
        private static string _VersionText = AppVersion;

        [ObservableProperty]
        private static string _StatusText = String.Format(Translate.GetText("txt_Disabled", MainWindow._AppLanguage), MainWindow._AppName);

        [ObservableProperty]
        private static string _WindowBackground = Color1;

        [ObservableProperty]
        private static string _TextBlockBackground = Transparent;

        [ObservableProperty]
        private static string _TextBlockForeground = Color2;

        [ObservableProperty]
        private static string _TextBlockShadow = Color3;

        [ObservableProperty]
        public static WindowState _WinState = WindowState.Normal;

        [ObservableProperty]
        private Double _StatusFontSize = 20;

        [ObservableProperty]
        private Double _VersionFontSize = 8;

        [ObservableProperty]
        public static Point _ToggleButtonTopLeft = new();

        [ObservableProperty]
        public static string _ToggleButtonCaption = String.Format(Translate.GetText("txt_Start", MainWindow._AppLanguage), MainWindow._AppName);

        [ObservableProperty]
        public static bool _MouseMove = false;

        #pragma warning restore S2223, S1104, CS8602, CA2211
        #endregion

        /// <summary>
        /// Start object
        /// </summary>
        public MainWindow()
        {
            //set UI language according to system language
            if (CultureInfo.CurrentCulture.ThreeLetterISOLanguageName.Equals("deu", StringComparison.InvariantCultureIgnoreCase))
                AppLanguage = "de-de";
            else
                AppLanguage = "en-us";

            //Initialize window
            InitializeComponent();

            //add datacontext and events
            DataContext = this;
            MouseDown += MouseDownHandler;
            MouseUp += MouseUpHandler;
            MouseDoubleClick += MouseDoubleClickHandler;
            Deactivated += DeactivatedHandler;
            SizeChanged += SizedHandler;
            LayoutUpdated += StateHandler;

            //Set initial tooltip texts
            SetToolTipTexts();
        }

        /// <summary>
        /// Resize event
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void SizedHandler(object? sender, EventArgs e)
        {
            if (this.WinState == WindowState.Normal)
            {
                this.MainGrid.RowDefinitions[0].Height = new GridLength(40, GridUnitType.Pixel);
                this.WindowControlsGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                this.MainGrid.RowDefinitions[0].Height = new GridLength(80, GridUnitType.Pixel);
                this.WindowControlsGrid.RowDefinitions[0].Height = new GridLength(15, GridUnitType.Pixel);
            }
            this.WindowControlsGrid.ColumnDefinitions[1].Width = this.MainGrid.RowDefinitions[0].Height;
            this.WindowControlsGrid.ColumnDefinitions[2].Width = this.MainGrid.RowDefinitions[0].Height;
            this.WindowControlsGrid.ColumnDefinitions[3].Width = this.MainGrid.RowDefinitions[0].Height;
            this.WindowControlsGrid.ColumnDefinitions[4].Width = this.MainGrid.RowDefinitions[0].Height;
            this.WindowControlsGrid.ColumnDefinitions[5].Width = this.MainGrid.RowDefinitions[0].Height;
            this.WindowControlsGrid.ColumnDefinitions[6].Width = this.MainGrid.RowDefinitions[0].Height;
            ToggleButtonTopLeft = this.StartStopButton.PointToScreen(new Point(0, 0));
            this.StartStopButton.CornerRadius = (int)Math.Round(this.TextBlock1.ActualHeight / 10);
        }

        /// <summary>
        /// WindowState handler used for FontSize calculation
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void StateHandler(object? sender, EventArgs e)
        {
            if (this.TextBlock1 != null && !string.IsNullOrEmpty(StatusText))
            {
                if (Math.Round(this.TextBlock1.ActualWidth / 10) < 35791)
                    StatusFontSize = Math.Round(this.TextBlock1.ActualWidth / 11);
                else
                    StatusFontSize = Math.Round(this.TextBlock1.ActualWidth / 10);
            }
            if (this.TextBlock1 != null && this.StartStopButton != null)
                this.StartStopButton.CornerRadius = (int)Math.Round(this.TextBlock1.ActualHeight / 10);
            if (this.VersionTextBlock != null)
                VersionFontSize = Math.Round(this.VersionTextBlock.ActualWidth / 5.5);
        }

        /// <summary>
        /// Keep window topmost
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void DeactivatedHandler(object? sender, EventArgs e)
        {
            this.Topmost = StayOnTopButton.IsChecked;
            this.BringIntoView();
            this.Activate();
            this.Focus();
            ToggleButtonTopLeft = this.StartStopButton.PointToScreen(new Point(0, 0));
            this.StartStopButton.CornerRadius = (int)Math.Round(this.TextBlock1.ActualHeight / 10);
        }

        /// <summary>
        /// EventHandler MouseUp
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            //reset mouse cursor
            this.Cursor = Cursors.Arrow;
            ToggleButtonTopLeft = this.StartStopButton.PointToScreen(new Point(0, 0));
            e.Handled = true;
        }

        /// <summary>
        /// EventHandler MouseDown
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            //change mouse cursor and do window drag move
            this.Cursor = Cursors.Hand;
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
            e.Handled = true;
        }

        /// <summary>
        /// EventHandler MouseDoubleClick (for future use)
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void MouseDoubleClickHandler(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        [RelayCommand]
        private static void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Set tooltip texts to according translation
        /// </summary>
        public static void SetToolTipTexts()
        {
            //ToolTips
            if (App.Current == null)
                return;
            #pragma warning disable CS8602
            (App.Current.MainWindow.FindName("ExitButton") as ExitButton).ToolTip = String.Format(Translate.GetText("txt_Exit", MainWindow._AppLanguage), MainWindow._AppName);
            (App.Current.MainWindow.FindName("MaxMinButton") as MaxMinButton).ToolTip = Translate.GetText("txt_Fullscreen", MainWindow._AppLanguage);
            (App.Current.MainWindow.FindName("MinimizeButton") as MinimizeButton).ToolTip = Translate.GetText("txt_Minimize", MainWindow._AppLanguage);
            (App.Current.MainWindow.FindName("StayOnTopButton") as StayOnTopButton).ToolTip = Translate.GetText("txt_StayOnTop", MainWindow._AppLanguage);
            (App.Current.MainWindow.FindName("MouseMoveButton") as MouseMoveButton).ToolTip = Translate.GetText("txt_MouseMove", MainWindow._AppLanguage);
            (App.Current.MainWindow.FindName("LanguageButton") as LanguageButton).ToolTip = Translate.GetText("txt_Language", MainWindow._AppLanguage);
            (App.Current.MainWindow.FindName("StartStopButton") as ToggleButton).ToolTip = String.Format(Translate.GetText("txt_StartStop", MainWindow._AppLanguage), MainWindow._AppName);
            #pragma warning restore CS8602
        }
    }
}