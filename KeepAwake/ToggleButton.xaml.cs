using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeepAwake.Assets.Translation;

namespace KeepAwake
{
    /// <summary>
    /// Interaktionslogik für ToggleButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class ToggleButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public ToggleButton()
        {
            InitializeComponent();

            //add datacontext and events
            DataContext = this;
            SizeChanged += SizedHandler;
        }

        #region Definitions and Variables
        [ObservableProperty]
        private static string _Caption = String.Format(Translate.GetText("txt_Start", MainWindow._AppLanguage), MainWindow._AppName);

        [ObservableProperty]
        private String _Status = String.Format(Translate.GetText("txt_Disabled", MainWindow._AppLanguage), MainWindow._AppName);

        [ObservableProperty]
        private Boolean _Checked = false;

        [ObservableProperty]
        private Double _CaptionFontSize = 22;

        [ObservableProperty]
        private int _CornerRadius = 5;      

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }
        #endregion

        /// <summary>
        /// Resize the button caption font size
        /// </summary>
        /// <param name="sender">sender as button</param>
        /// <param name="e">event arguments</param>
        private void SizedHandler(object? sender, EventArgs e)
        {
            var togglebutton = (sender as ToggleButton);
            if (togglebutton == null)
                CaptionFontSize = 22;
            else
                CaptionFontSize = Math.Round(togglebutton.ActualWidth / 9);
        }

        /// <summary>
        /// Start the KeepAwake task which prevents by execution state signaling that the system goes in idle
        /// </summary>
        /// <param name="ct">CancellationToken Source</param>
        /// <returns></returns>
        async Task KeepAwake(CancellationToken ct)
        {
            try
            {
                #pragma warning disable S125
                /// Different Spinner codes - choose your flavor
                //Spinner Star 1
                //char[] SpinnerChars = new char[4]{ '-', '\\', '|', '/' };
                //Spinner Star 2
                //char[] SpinnerChars = new char[4] { '', '\\', '|', '/' };
                //Spinner Clock 1
                //char[] SpinnerChars = new char[4] { '', '', '', '' };
                //Spinner Clock 2 
                char[] SpinnerChars = ['', '', '', '', '', '', '', '', '', '', '', ''];
                //Spinner Sandclock
                //char[] SpinnerChars = new char[4] { '', '', '', '' };
                //Spinner Arrows
                //char[] SpinnerChars = new char[4] { '', '', '', '' };
                //Spinner Bar
                //char[] SpinnerChars = new char[13] { ' ', '▂', '▃', '▄', '▅', '▆', '▇', '█', '▇', '▆', '▄', '▃', '▂' };
                #pragma warning restore S125

                //variables
                byte SpinnerPos = 0;
                Random random = new(DateTime.Now.Millisecond);

                //Signaling loop
                while (true)
                {
                    try
                    {
                        //Keep system alive
                        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);

                        //optional mouse move
                        if (MainWindow._MouseMove)
                            SetCursorPos((int)(MainWindow._ToggleButtonTopLeft.X + this.ActualWidth / 2 + random.Next((int)(this.ActualWidth / 2))), (int)(MainWindow._ToggleButtonTopLeft.Y + this.ActualHeight / 2 + random.Next((int)(this.ActualHeight / 2))));

                        //delay and spinner before next loop round
                        if (ct.IsCancellationRequested || !Checked) break;
                        for (int i = 0; i < 50; i++)
                        {
                            Status = String.Format(Translate.GetText("txt_Running", MainWindow._AppLanguage), MainWindow._AppName) + " " + SpinnerChars[SpinnerPos];
                            SpinnerPos++;
                            if (SpinnerPos >= SpinnerChars.Length)
                                SpinnerPos = 0;
                            if (ct.IsCancellationRequested || !Checked) break;
                            await Task.Delay(200, CancellationToken.None);
                            if (ct.IsCancellationRequested || !Checked) break;
                        }
                        if (ct.IsCancellationRequested || !Checked) break;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                        break;
                    }
                }
            }
            finally
            {
                Caption = String.Format(Translate.GetText("txt_Start", MainWindow._AppLanguage), MainWindow._AppName);
                Status = String.Format(Translate.GetText("txt_Disabled", MainWindow._AppLanguage), MainWindow._AppName);
                Checked = false;
            }
        }

        /// <summary>
        /// Toggle the checked state
        /// </summary>
        [RelayCommand]
        private void Toggle()
        {
            if (Checked)
            {
                App.Ct = new CancellationTokenSource();
                Task.Run(() => KeepAwake(App.Ct.Token));
                Caption = String.Format(Translate.GetText("txt_Stop", MainWindow._AppLanguage), MainWindow._AppName);
            }
            else
            {
                if (App.Ct != null)
                {
                    App.Ct.Cancel();
                    App.Ct.Dispose();
                }
                Caption = String.Format(Translate.GetText("txt_Start", MainWindow._AppLanguage), MainWindow._AppName);
                Status = String.Format(Translate.GetText("txt_Disabled", MainWindow._AppLanguage), MainWindow._AppName);
            }
        }
    }
}
