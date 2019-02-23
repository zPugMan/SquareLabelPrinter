﻿using System;
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

        public IEnumerable<string> Categories
        {
            get
            {
                List<string> categories = new List<string>();
                categories.Add("Natural Tech");
                categories.Add("Oi");
                categories.Add("Essentials");
                categories.Add("Alchemic");
                return categories;
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
                p.Category = "Natural Tech";
                p.Name = "Calming Shampoo";
                p.Size = "Regular 250ml";
                p.SKU = "DLCURL250";
                p.Price = "$53.02";
                return p;
            }
            set
            {
                selectedProduct = value;
            }
        }
    }
}
