using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KeepAwake
{
    /// <summary>
    /// Interaktionslogik für ExitButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class ExitButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public ExitButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Exit application
        /// </summary>
        [RelayCommand]
        private static void Exit()
        {
            if (App.Ct != null && !App.Ct.IsCancellationRequested)
            {
                try
                {
                    App.Ct.Cancel();
                    App.Ct.Dispose();
                }
                catch
                {
                    //no action needed before quitting the application
                }
            }
            Environment.Exit(0);
        }
    }
}
