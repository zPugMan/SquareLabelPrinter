using RetailAppWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Messages;
using RetailAppWPF.Commands;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace RetailAppWPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private NavigationStore navigationStore;
        private readonly ILoggerFactory _loggerFactory;
        public SettingsViewModel(ILoggerFactory loggerFactory,NavigationStore navStore, Notifier notify)
        {
            _loggerFactory = loggerFactory; 
            NotifyToast = notify;
            navigationStore = navStore;
            SettingCancelCommand = new NavigateCommand<CatalogPrintViewModel>(navStore, () => new CatalogPrintViewModel(loggerFactory, navStore, notify));

            SelectedEnvironment = SettingsStore.Environment;
            AccessToken = SettingsStore.AccessToken;
            SelectedLocation = SettingsStore.Location;
            PrintEnabled = SettingsStore.PrintEnable;
        }

        public readonly ObservableCollection<string> AvailableEnvironments = new ObservableCollection<string>() { "Production", "Sandbox" };
        public string SelectedEnvironment { get; set; }

        public bool PrintEnabled { get; set; }

        private string _accessToken;
        public string AccessToken
        {
            get { return _accessToken; }
            set
            {
                if(!string.IsNullOrEmpty(value) && !value.Equals(_accessToken, StringComparison.CurrentCulture))
                {
                    _accessToken = value;
                }
            }
        }

        private List<string> _location;
        public List<string> AvailableLocations { get; set; }

        public string SelectedLocation { get; set; }

        private Helper.RelayCommand settingSaveCommmand;
        public ICommand SettingSaveCommand
        {
            get
            {
                if (settingSaveCommmand == null)
                {
                    settingSaveCommmand = new Helper.RelayCommand(
                        p => true,
                        p => this.SaveSettings());
                }
                return settingSaveCommmand;
            }
        }

        private void SaveSettings()
        {
            if (!(SelectedEnvironment.Equals("Sandbox") || SelectedEnvironment.Equals("Production")))
            {
                NotifyToast.ShowError("Environment invalid: Sandbox or Production");
                return;
            }
                
            Properties.Settings.Default.Environment = SelectedEnvironment;
            Properties.Settings.Default.AccessToken = AccessToken;
            Properties.Settings.Default.Location = SelectedLocation;
            Properties.Settings.Default.PrintEnable = PrintEnabled;

            try
            {
                Properties.Settings.Default.Save();
                NotifyToast.ShowInformation("Settings saved");
                navigationStore.CurrentViewModel = new CatalogPrintViewModel(_loggerFactory,navigationStore, NotifyToast);
            } catch(Exception e)
            {
                NotifyToast.ShowError("Error on save: " + e.Message);
                return;
            }
            
        }

        public ICommand SettingCancelCommand { get; }
    }
}
