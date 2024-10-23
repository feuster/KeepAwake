using System.Windows;

namespace KeepAwake
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //CancellationToken for AsyncTask
        public static CancellationTokenSource Ct { get; set; } = new CancellationTokenSource();
    }

}
