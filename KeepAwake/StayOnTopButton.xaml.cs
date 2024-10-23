using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
namespace KeepAwake
{

    /// <summary>
    /// Interaktionslogik für StayOnTopButton.xaml
    /// </summary>
    [ObservableObject]
    public partial class StayOnTopButton : UserControl
    {
        /// <summary>
        /// Init
        /// </summary>
        public StayOnTopButton()
        {
            DataContext = this;
            InitializeComponent();
        }

        [ObservableProperty]
        public Boolean _IsChecked = true;

        /// <summary>
        /// Toggle if window stays on top or not
        /// </summary>
        [RelayCommand]
        private void Toggle()
        {
            IsChecked = !IsChecked;
            App.Current.MainWindow.UpdateLayout();
        }
    }
}
