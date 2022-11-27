using RetailAppWPF.Commands;
using RetailAppWPF.Models;
using RetailAppWPF.Services;
using RetailAppWPF.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Messages;

namespace RetailAppWPF.ViewModels
{
    /// <summary>
    /// View model for the catalog print label request
    /// </summary>
    public class CatalogPrintViewModel : ViewModelBase
    {
        private static CatalogService catalog;
        
        public CatalogPrintViewModel(NavigationStore navStore, Notifier notify)
        {
            NotifyToast = notify;
            catalog = new CatalogService();
            Categories = catalog.GetProductCategories();
            updateInventory = false;
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navStore, ()=> new SettingsViewModel(navStore, notify));

            PropertyChanged += async (s,e) => await AsyncPropertyChanged(s,e);
        }

        public ICommand NavigateSettingsCommand { get; }
        public Helper.RelayCommand forceRefreshCommand;
        public ICommand ForceRefreshCommand
        {
            get
            {
                forceRefreshCommand = new Helper.RelayCommand(
                    p => true,
                    p => this.ForceRefresh()
                    );
                return forceRefreshCommand;
            }
        }

        public void ForceRefresh()
        {
            SelectedCategory = null;
            Products = null;
            Categories = catalog.GetProductCategories(forceRefresh: true);
            NotifyToast.ShowInformation("Forced refresh from Square");
        }

        private IEnumerable<String> categories;
        /// <summary>
        /// List of available product categories
        /// </summary>
        public IEnumerable<string> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }

        private string selectedCategory;
        /// <summary>
        /// Current, selected product category
        /// </summary>
        public string SelectedCategory
        {
            get
            {
                return selectedCategory;
            }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
                
            }
        }

        private async Task AsyncPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCategory))
            {
                if(selectedCategory != null)
                    Products = await catalog.GetProducts(selectedCategory);
            }
                
        }

        private IEnumerable<ProductItem> products;
        /// <summary>
        /// List of products within a product category as defined by <see cref="SelectedCategory"/>
        /// </summary>
        public IEnumerable<ProductItem> Products
        {
            get
            {
                if (products == null)
                    return new List<ProductItem>();
                else
                    return products;
            }
            set
            {
                products = value;
                OnPropertyChanged("Products");
            }
        }

        private ProductItem selectedProduct;
        /// <summary>
        /// Current, selected product
        /// </summary>
        public ProductItem SelectedProduct
        {
            get
            {
                return selectedProduct;
            }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        private int printQuantity;
        /// <summary>
        /// Set number of labels to print
        /// </summary>
        public int PrintQuantity
        {
            get { return printQuantity; }
            set
            {
                printQuantity = value;
                OnPropertyChanged("PrintQuantity");
            }
        }

        private bool updateInventory;
        /// <summary>
        /// Popup control for prompt when true
        /// </summary>
        public bool UpdateInventoryCheck
        {
            get { return updateInventory; }
            set
            {
                if (updateInventory != value) { }
                    Thread.Sleep(150);

                updateInventory = value;
                OnPropertyChanged(nameof(UpdateInventoryCheck));
            }
        }

        private Helper.RelayCommand printLabelCommand;
        public ICommand PrintLabelCommand
        {
            get
            {
                if (printLabelCommand == null)
                {
                    printLabelCommand = new Helper.RelayCommand(
                        p => this.CanPrint,
                        p => this.PrintLabel()
                        );
                }
                return printLabelCommand;
            }
        }

        public bool CanPrint
        {
            get
            {
                if (CanPlusMinus && PrintQuantity > 0)
                    return true;
                else
                    return false;
            }
        }

        public void PrintLabel()
        {
            if (!SettingsStore.PrintEnable)
            {
                UpdateInventoryCheck = true;
                return;
            }

            using (PrintServices print = new PrintServices())
            {
                UpdateInventoryCheck = true;
                print.PrintBarcodeLabel2(SelectedProduct, PrintQuantity);
            }
        }

        private Helper.RelayCommand addPrintQuantityCommand;
        /// <summary>
        /// Adds 1 to the PrintQuantity
        /// </summary>
        public ICommand AddPrintQuantityCommand
        {
            get
            {
                if (addPrintQuantityCommand == null)
                {
                    addPrintQuantityCommand = new Helper.RelayCommand(
                        p => this.CanPlusMinus,
                        p => this.AddPrintQuantity()
                        );
                }
                return addPrintQuantityCommand;
            }
        }

        private Helper.RelayCommand subtractPrintQuantityCommand;
        /// <summary>
        /// Subracts 1 to the PrintQuantity
        /// </summary>
        public ICommand SubtractPrintQuantityCommand
        {
            get
            {
                if (subtractPrintQuantityCommand == null)
                {
                    subtractPrintQuantityCommand = new Helper.RelayCommand(
                        p => this.CanPlusMinus,
                        p => this.SubtractPrintQuantity()
                        );
                }
                return subtractPrintQuantityCommand;
            }
        }

        private Helper.RelayCommand updateInventoryCommand;
        public ICommand UpdateInventoryCommand
        {
            get
            {
                updateInventoryCommand = new Helper.RelayCommand(
                    p => this.printQuantity > 0,
                    p => this.ExecuteInventoryUpdate());
                return updateInventoryCommand;
            }
        }

        private Helper.RelayCommand cancelUpdateInventoryCommand;
        public ICommand CancelUpdateInventoryCommand
        {
            get
            {
                cancelUpdateInventoryCommand = new Helper.RelayCommand(
                    p => true,
                    p => this.Reset());
                return cancelUpdateInventoryCommand;
            }
        }

        private void ExecuteInventoryUpdate()
        {
            if (SelectedProduct == null)
                return;

            if (PrintQuantity < 1)
                return;

            var svc = new InventoryManagerService();
            var result = svc.AddInventory(SelectedProduct, PrintQuantity);
            if (result.IsSuccess)
                NotifyToast.ShowSuccess(result.Message);
            else
                NotifyToast.ShowWarning(result.Message);

            UpdateInventoryCheck = !result.IsSuccess;
        }

        public void AddPrintQuantity()
        {
            PrintQuantity++;
        }

        public void SubtractPrintQuantity()
        {
            PrintQuantity--;
        }

        /// <summary>
        /// Validates the ability to add / subtract to the print quantity. Only possible
        /// if a SelectedProduct is defined.
        /// </summary>
        public bool CanPlusMinus
        {
            get
            {
                if (SelectedProduct != null)
                    return true;
                else
                    return false;
            }
        }

        public void Reset()
        {
            UpdateInventoryCheck = false;
            PrintQuantity = 0;
        }

        

    }
}
