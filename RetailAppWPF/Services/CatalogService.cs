using ExcelDataReader;
using RetailApp.Data;
using RetailAppWPF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppWPF.Services
{
    public class CatalogService
    {
        private System.Data.DataTable catalog;
        private IEnumerable<RetailAppWPF.Models.ProductItem> products;

        public CatalogService()
        {
            //var excelFilePath = Path.Combine(Environment.CurrentDirectory, @"Assets\catalog-square.xlsx");

            //using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            //{
            //    using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
            //    {
            //        var conf = new ExcelDataSetConfiguration
            //        {
            //            ConfigureDataTable = _ => new ExcelDataTableConfiguration
            //            {
            //                UseHeaderRow = true
            //            }
            //        };

            //        var dataSet = reader.AsDataSet(conf);
            //        catalog = dataSet.Tables[0];

            //        LoadProducts();
            //    }
            //}
            LoadCategories();
        }

        private void LoadProducts()
        {
            if(catalog!=null && catalog.Rows.Count > 0)
            {
                products = catalog.Rows.Cast<DataRow>().Select(
                   x=> new ProductItem
                   {
                       Name = x.Field<string>("Item Name"),
                       Size = x.Field<string>("Variation Name"),
                       SKU = x.Field<string>("SKU"),
                       Category = x.Field<string>("Category"),
                       Price = x.Field<string>("Price")
                   }
                );

            }
        }

        public IEnumerable<ProductItem> GetProducts(string category)
        {
            //return products.Where(x => x.Category == category);
            RetailApp.Data.SquareCatalogService svc = new RetailApp.Data.SquareCatalogService();
            List<SquareProduct> products = svc.GetCatelogItemsByCategory(category);
            return ProductItem.ToList(products);
        }

        private List<SquareCategory> squareCategories;
        public IEnumerable<string> GetProductCategories()
        {
            //return products.Select(x => x.Category).Distinct();
            return squareCategories.Select(x => x.CategoryName).Distinct();
        }

        /// <summary>
        /// Loads the product categories directly from Square
        /// </summary>
        /// <returns></returns>
        public void LoadCategories()
        {
            RetailApp.Data.SquareCatalogService svc = new RetailApp.Data.SquareCatalogService();
            List<SquareCategory> response = svc.GetCategories();
            squareCategories = response;
        }

    }
}
