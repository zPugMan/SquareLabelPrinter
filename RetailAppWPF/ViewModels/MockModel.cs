using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailAppWPF.Models;

namespace RetailAppWPF.ViewModels
{
    public class MockModel
    {
        public MockModel()
        {

        }

        private IEnumerable<string> categories;
        public IEnumerable<string> Categories
        {
            get
            {
                List<String> categories = new List<string>();
                categories.Add("Natural Tech");
                categories.Add("Oi");
                categories.Add("Essentials");
                categories.Add("Alchemic");
                categories.Add("Styling");
                return categories;
            }
            set
            {
                categories = value;
            }
        }

        private IEnumerable<ProductItem> products;
        public IEnumerable<ProductItem> Products
        {
            get
            {
                List<ProductItem> products = new List<ProductItem>();
                ProductItem p = new ProductItem();
                p.Category = "Essentials";
                p.Name = "Love Curl Shampoo";
                p.Size = "Regular 250ml";
                p.SKU = "DLCURL250";
                p.Price = "$53.02";
                products.Add(p);

                p = new ProductItem();
                p.Category = "Essentials";
                p.Name = "Love Smooth Shampoo";
                p.Size = "Regular 250ml";
                p.SKU = "DLCURL250";
                p.Price = "$53.02";
                products.Add(p);

                p = new ProductItem();
                p.Category = "Natural Tech";
                p.Name = "Calming Shampoo";
                p.Size = "Regular 250ml";
                p.SKU = "DLCURL250";
                p.Price = "$53.02";
                products.Add(p);

                p = new ProductItem();
                p.Category = "Styling";
                p.Name = "EXTRA Strong Hold hair spray";
                p.SKU = "MIESHHS400";
                p.Size = "Regular";
                p.Price = "$35.00";
                products.Add(p);
                return products;
            }
            set
            {
                products = value;
            }
        }

        private ProductItem selectedProduct;
        public ProductItem SelectedProduct
        {
            get
            {
                ProductItem p = new ProductItem();
                p.Category = "Styling";
                p.Name = "EXTRA Strong Hold hair spray";
                p.SKU = "MIESHHS400";
                p.Size = "Regular";
                p.Price = "$35.00";
                return p;
            }
            set
            {
                selectedProduct = value;
            }
        }

        private string printQuantity;
        public string PrintQuantity
        {
            get { return "1"; }
            set { printQuantity = value; }
        }
    }
}
