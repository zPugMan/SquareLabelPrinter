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
    public class CatalogPrintViewModel : INotifyPropertyChanged
    {
        private static CatalogService catalog;

        public CatalogPrintViewModel()
        {
            catalog = new CatalogService();
            Categories = catalog.GetProductCategories();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<String> categories;
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
        public int PrintQuantity
        {
            get { return printQuantity; }
            set
            {
                printQuantity = value;
                OnPropertyChanged("PrintQuantity");
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
                print.PrintBarcodeLabel(SelectedProduct, PrintQuantity);
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
