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

        public ICommand NavigateCatalogPrintCommand { get; }

        public SettingsViewModel(NavigationStore navStore, Notifier notify)
        {
            NotifyToast = notify;
            NavigateCatalogPrintCommand = new NavigateCommand<CatalogPrintViewModel>(navStore, () => new CatalogPrintViewModel(navStore, notify));
        }
    }
}
