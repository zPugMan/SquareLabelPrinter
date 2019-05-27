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

        //List<CatalogItem> CatalogItems(String categoryID);

    }
}
