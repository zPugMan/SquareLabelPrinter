using RetailAppWPF.Models;
using RetailAppWPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailAppWPF.ViewModels
{
    /// <summary>
    /// View model for the catalog print label request
    /// </summary>
    public class CatalogPrintViewModel : INotifyPropertyChanged
    {
        private static CatalogService catalog;

        public CatalogPrintViewModel()
        {
            catalog = new CatalogService();
            Categories = catalog.GetProductCategories();
            updateInventory = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                Products = catalog.GetProducts(selectedCategory);
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

        private string updateInventoryResult;
        public string UpdateInventoryResult
        {
            get { return updateInventoryResult; }
            set
            {
                if (!updateInventoryResult.Equals(value))
                {
                    updateInventoryResult = value;
                    OnPropertyChanged(nameof(UpdateInventoryResult));
                }
            }
        }


        private bool updateInventory;
        public bool UpdateInventoryCheck
        {
            get { return updateInventory; }
            set
            {
                updateInventory = value;
                OnPropertyChanged(nameof(UpdateInventoryCheck));
            }
        }

        private Helper.RelayCommand printLabelCommand;
        public ICommand PrintLabelCommand
        {
            get
            {
                if(printLabelCommand == null)
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
            using (PrintServices print = new PrintServices())
            {
                UpdateInventoryCheck = true;
                //print.PrintBarcodeLabel2(SelectedProduct, PrintQuantity);
            }
            SelectedProduct = null;
        }

        private Helper.RelayCommand addPrintQuantityCommand;
        /// <summary>
        /// Adds 1 to the PrintQuantity
        /// </summary>
        public ICommand AddPrintQuantityCommand
        {
            get
            {
                if(addPrintQuantityCommand == null)
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
                if(subtractPrintQuantityCommand == null)
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
            UpdateInventoryResult = string.Empty;
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

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
