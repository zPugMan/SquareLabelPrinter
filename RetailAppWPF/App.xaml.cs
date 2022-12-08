using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RetailAppWPF.Stores;
using RetailAppWPF.ViewModels;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using Serilog;

namespace RetailAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            
            ILoggerFactory logFactory = LoggerFactory.Create(build =>
            {
                build.AddDebug();

                LoggerConfiguration logConfig = new LoggerConfiguration()
                    .WriteTo.File("action.log");
                build.AddSerilog(logConfig.CreateLogger());
            });
            services.AddSingleton(logFactory);
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            
            Notifier notify = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;


            });

            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            if (string.IsNullOrEmpty(RetailAppWPF.Properties.Settings.Default.AccessToken))
            {
                navigationStore.CurrentViewModel = new SettingsViewModel(logFactory, navigationStore, notify);
            }
            else
            {
                navigationStore.CurrentViewModel = new CatalogPrintViewModel(logFactory, navigationStore, notify);
            }

            //MainWindow = new MainWindow()
            //{
            //    DataContext = new MainViewModel(navigationStore, notify)
            //};
            //MainWindow.Show();
            var main = serviceProvider.GetService<MainWindow>();
            main.DataContext = new MainViewModel(navigationStore, notify);
            main.Show();

            base.OnStartup(e);
        }
    }
}
