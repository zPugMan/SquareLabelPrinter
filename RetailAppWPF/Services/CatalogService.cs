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
        private IEnumerable<RetailAppWPF.Models.ProductItem> products;

        public CatalogService()
        {
            LoadCategories();
        }

        public IEnumerable<ProductItem> GetProducts(string category)
        {
            RetailApp.Data.SquareCatalogService svc = new RetailApp.Data.SquareCatalogService();
            List<SquareProduct> products = svc.GetCatelogItemsByCategory(category);
            return ProductItem.ToList(products).OrderBy( p=>p.Price).OrderBy(p => p.Name);
        }

        private List<SquareCategory> squareCategories;
        public IEnumerable<string> GetProductCategories()
        {
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
