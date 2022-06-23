using RetailAppWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;
using RetailAppWPF.Commands;

namespace RetailAppWPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        public ICommand SettingSaveCommand { get; }
        public ICommand SettingCancelCommand { get; }

        public SettingsViewModel(NavigationStore navStore, Notifier notify)
        {
            NotifyToast = notify;
            SettingSaveCommand = new NavigateCommand<CatalogPrintViewModel>(navStore, () => new CatalogPrintViewModel(navStore, notify));
            SettingCancelCommand = new NavigateCommand<CatalogPrintViewModel>(navStore, () => new CatalogPrintViewModel(navStore, notify));
        }
    }
}
