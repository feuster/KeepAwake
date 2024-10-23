using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeepAwake.Assets.Translation;
using System.Windows.Controls;

namespace KeepAwake
{

    /// <summary>
    /// Interaktionslogik für MouseMoveButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class LanguageButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public LanguageButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        [ObservableProperty]
        public string _LanguageCode = MainWindow._AppLanguage;

        /// <summary>
        /// Switch displayed language
        /// </summary>
        [RelayCommand]
        private void Toggle()
        {
            //Switch language or fallback to en-us as default
            LanguageCode = LanguageCode.ToLowerInvariant() switch
            {
                ("en-us") => "de-de",
                ("de-de") => "en-us",
                _ => "en-us",
            };
            MainWindow._AppLanguage = LanguageCode;

            //Update texts with translation
            var ToggleButton = (App.Current.MainWindow.FindName("StartStopButton") as ToggleButton);
            if (ToggleButton != null)
            { 
                if (ToggleButton.Checked)
                {
                    ToggleButton.Caption = String.Format(Translate.GetText("txt_Stop", MainWindow._AppLanguage), MainWindow._AppName);
                }
                else
                {
                    ToggleButton.Caption = String.Format(Translate.GetText("txt_Start", MainWindow._AppLanguage), MainWindow._AppName);
                    ToggleButton.Status = String.Format(Translate.GetText("txt_Disabled", MainWindow._AppLanguage), MainWindow._AppName);
                }
            }
            MainWindow.SetToolTipTexts();
        }
    }
}
