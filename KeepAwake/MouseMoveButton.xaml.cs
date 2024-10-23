using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;

namespace KeepAwake
{

    /// <summary>
    /// Interaktionslogik für MouseMoveButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class MouseMoveButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public MouseMoveButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        [ObservableProperty]
        public Boolean _IsChecked = false;

        /// <summary>
        /// Enable/disable mouse move simulation
        /// </summary>
        [RelayCommand]
        private void Toggle()
        {
            IsChecked = !IsChecked;
            MainWindow._MouseMove = IsChecked;
        }
    }
}
