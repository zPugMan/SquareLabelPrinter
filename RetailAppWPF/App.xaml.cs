using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RetailAppWPF.Stores;
using RetailAppWPF.ViewModels;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace RetailAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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

            if (string.IsNullOrEmpty(RetailAppWPF.Properties.Settings.Default.AccessToken))
                navigationStore.CurrentViewModel = new SettingsViewModel(navigationStore, notify);
            else
                navigationStore.CurrentViewModel = new CatalogPrintViewModel(navigationStore, notify);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore, notify)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
