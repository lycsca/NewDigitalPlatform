using NewDigitalPlatform.ViewModels;
using NewDigitalPlatform.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NewDigitPlatform.Start
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DependencyInjection.ConfigureServices();
           //var mainWindow = DependencyInjection.GetService<MainWindow>();
           // mainWindow.Show();

        }
    }

}
