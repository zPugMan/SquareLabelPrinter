using RetailApp.Data;
using RetailAppWPF.Models;
using RetailAppWPF.Stores;
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
            if(SettingsStore.IsReady())
                LoadCategories();
        }

        public async Task<IEnumerable<ProductItem>> GetProducts(string category)
        {
            RetailApp.Data.SquareCatalogService svc = new RetailApp.Data.SquareCatalogService(
                location: SettingsStore.Location,
                accessToken: SettingsStore.AccessToken,
                environment: SettingsStore.Environment
                );
            List<SquareProduct> products = await svc.GetCatalogItemsByCategoryAsync(category);
            return ProductItem.ToList(products).OrderBy( p=>p.Price).OrderBy(p => p.Name);
        }

        private List<SquareCategory> squareCategories;
        public IEnumerable<string> GetProductCategories(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                squareCategories.Clear();
                LoadCategories();
            }
            return squareCategories.OrderBy(y => y.CategoryName).Select(x=>x.CategoryName).Distinct();
        }

        /// <summary>
        /// Loads the product categories directly from Square
        /// </summary>
        /// <returns></returns>
        public void LoadCategories()
        {
            RetailApp.Data.SquareCatalogService svc = new RetailApp.Data.SquareCatalogService(
                location: SettingsStore.Location,
                accessToken: SettingsStore.AccessToken,
                environment: SettingsStore.Environment
                );
            List<SquareCategory> response = svc.GetCategories();
            squareCategories = response;
        }

    }
}
