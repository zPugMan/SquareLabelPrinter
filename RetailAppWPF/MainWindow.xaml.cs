using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RetailAppWPF.Services;
using RetailAppWPF.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.ComponentModel;

namespace RetailAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private static CatalogService catalog;
        private IEnumerable<ProductItem> products;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            catalog = new CatalogService();
            this.DataContext = this;
            PrintQuantity = 1;
        }

        public IEnumerable<string> Categories
        {
            get { return catalog.GetProductCategories();  }
        }

        public IEnumerable<ProductItem> Products
        {
            get {
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

        public string SelectedCategory { get; set; }
        private ProductItem selectedProduct;
        public ProductItem SelectedProduct {
            get { return selectedProduct; }
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

        private int GetPrintQuantity()
        {
            //int result = 1;
            //int.TryParse(PrintQuantity.ToString(), out result);
            //return result;
            return 0;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            using (PrintServices print = new PrintServices())
            {

                if (SelectedProduct != null)
                {
                    //print.PrintBarcodeLabel2(SelectedProduct, GetPrintQuantity());
                    print.PrintBarcodeLabel2(SelectedProduct, PrintQuantity);
                }
            }
            
        }

        private void ListCategories_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                string category = e.AddedItems[0].ToString();// listCategories.SelectedItem.ToString();
                Products = catalog.GetProducts(category);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged; 
            if(handler!=null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void bttnAddQuantity_Click(object sender, RoutedEventArgs e)
        {
            //int qty;
            //int.TryParse(PrintQuantity, out qty);
            //PrintQuantity = String.Format("{0}", qty + 1);
            PrintQuantity = PrintQuantity + 1;
        }

        private void bttnSubtractQuantity_Click(object sender, RoutedEventArgs e)
        {
            //int qty;
            //int.TryParse(PrintQuantity, out qty);
            //PrintQuantity = String.Format("{0}", qty - 1);
            PrintQuantity = PrintQuantity - 1;
        }
    }
}
