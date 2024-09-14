using CommunityToolkit;
using DigitaPlatform.ViewModels;
using DigitaPlatForm.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using NewDigitalPlatform.DataAssets.Imp;
using NewDigitalPlatform.Models;
namespace NewDigitalPlatform.ViewModels
{
    public static class DependencyInjection
    {
        private static IServiceProvider serviceProvider;

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // 注册应用中的服务和ViewModel
            ///services.AddSingleton<MainWindow>();
            //services.AddTransient<IMyService, MyService>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<ILocalDataAccess,LocalDataAccess>();
            services.AddSingleton<UserModel>();
            serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }

    }

}
