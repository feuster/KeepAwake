using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace KeepAwake
{

    /// <summary>
    /// Interaktionslogik für MaxMinButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class MaxMinButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public MaxMinButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        [ObservableProperty]
        public Boolean _IsChecked = false;

        /// <summary>
        /// Toggle fullscreen or normal window mode
        /// </summary>
        [RelayCommand]
        private void Toggle()
        {
            IsChecked = !IsChecked;
            if (IsChecked)
                App.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                App.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }
}
