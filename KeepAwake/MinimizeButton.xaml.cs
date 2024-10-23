using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace KeepAwake
{
    /// <summary>
    /// Interaktionslogik für MinimizeButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class MinimizeButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public MinimizeButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Minimize the application into the taskbar
        /// </summary>
        [RelayCommand]
        private void Minimize()
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
