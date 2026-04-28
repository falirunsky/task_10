using System.Configuration;
using System.Data;
using System.Windows;
using task_10.services;
using task_10.viewModels;
using Microsoft.Extensions.DependencyInjection;

namespace task_10
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создание IoC контейнера
            var services = new ServiceCollection();

            // Singleton - один экземпляр на все приложение
            services.AddSingleton<IDialogService, DialogService>();

            // ТОже самое
            services.AddSingleton<MainViewModel>();

            // Window тоже через DI
            services.AddTransient<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // Запуск главного окна
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
