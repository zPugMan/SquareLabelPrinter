using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data
{
    public interface ICatalogService
    {
        Task<List<CatalogObject>> CatalogCategoriesAsync();
        List<CatalogObject> CatalogCategories();

        List<SquareProduct> GetCatalogItemsByCategory(string category);
        Task<List<SquareProduct>> GetCatalogItemsByCategoryAsync(string category);
        List<SquareCategory> GetCategories();
        Task<List<SquareCategory>> GetCategoriesAsync();

    }
}
