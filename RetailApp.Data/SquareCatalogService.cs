using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Square.Connect.Client;
using Square.Connect.Api;
using Square.Connect.Model;
using System.Configuration;

namespace RetailApp.Data
{
    public class SquareCatalogService : ICatalogService
    {
        private ICatalogApi catalogAPI;
        private readonly string Location = ConfigurationManager.AppSettings.Get("squareLocation");
        private readonly string BaseURL = ConfigurationManager.AppSettings.Get("squareClientUrl");
        private readonly string AccessToken = ConfigurationManager.AppSettings.Get("squareAccessToken");

        public SquareCatalogService()
        {
            Init();
        }

        public SquareCatalogService(ICatalogApi api)
        {
            catalogAPI = api; 
        }

        public SquareCatalogService(string location, string accessToken, string environment)
        {
            Location = location;
            AccessToken = accessToken;
            if (environment.Equals("Sandbox", StringComparison.CurrentCulture))
                BaseURL = "https://connect.squareupsandbox.com";
            else if (environment.Equals("Production", StringComparison.CurrentCulture))
                BaseURL = "https://connect.squareup.com";
            else
                throw new Exception("Invalid environment: " + environment);

            Init();
        }

        private void Init()
        {
            var config = new Square.Connect.Client.Configuration(new ApiClient(basePath: BaseURL));
            config.AccessToken = AccessToken;
            catalogAPI = new CatalogApi(config);
        }

        public async Task<List<CatalogObject>> CatalogCategoriesAsync()
        {
            var response = await catalogAPI.ListCatalogAsync(null, CatalogObjectType.CATEGORY.ToString());

            return response.Objects;
        }

        public List<CatalogObject> CatalogCategories()
        {
            try
            {
                var response = catalogAPI.ListCatalog(null, CatalogObjectType.CATEGORY.ToString());
                return response.Objects;
            } catch(Exception e)
            {
                return new List<CatalogObject>();
            }
            
        }

        public List<SquareProduct> GetCatelogItemsByCategory(string category)
        {
            SearchCatalogObjectsRequest req = new SearchCatalogObjectsRequest();
            req.IncludeRelatedObjects = true;
            req.ObjectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum>() { SearchCatalogObjectsRequest.ObjectTypesEnum.CATEGORY };
            req.Query = new CatalogQuery();
            req.Query.ExactQuery = new CatalogQueryExact(AttributeName: "name", AttributeValue: category);

            var response = catalogAPI.SearchCatalogObjects(req);

            string categoryID = string.Empty;
            List<SquareProduct> result = new List<SquareProduct>();
            if (response!=null)
            {
                categoryID = response.Objects[0].Id;
                req = new SearchCatalogObjectsRequest();
                req.IncludeRelatedObjects = true;
                req.ObjectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum>() { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
                req.Query = new CatalogQuery();
                req.Query.ExactQuery = new CatalogQueryExact(AttributeName: "category_id", AttributeValue: categoryID);

                var catalogItemsResponse = catalogAPI.SearchCatalogObjects(req);
                if (catalogItemsResponse != null && catalogItemsResponse.Objects.Count>0)
                {
                    
                    foreach(CatalogObject o in catalogItemsResponse.Objects)
                    {
                        result.AddRange(SquareProduct.LoadCatalogObject(o));
                    }
                }
                
            }

            return result;

        }

        //public List<CatalogObject> CatalogItems(String categoryID)
        //{
        //    SearchCatalogObjectsRequest request = GetCatalogByCategory(categoryID);
        //    var response = catalogAPI.SearchCatalogObjects(request);
        //    return response.Objects;
        //}

        //public async Task<List<CatalogObject>> CatalogItemsAsync(String categoryID)
        //{
        //    SearchCatalogObjectsRequest request = GetCatalogByCategory(categoryID);
        //    var response = await catalogAPI.SearchCatalogObjectsAsync(request);
        //    return response.Objects;
        //}

        //private SearchCatalogObjectsRequest GetCatalogByCategory(string categoryID)
        //{
        //    SearchCatalogObjectsRequest request = new SearchCatalogObjectsRequest();
        //    request.ObjectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum>() { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
        //    request.IncludeDeletedObjects = false;
        //    request.IncludeRelatedObjects = true;
        //    CatalogQuery searchQuery = new CatalogQuery();
        //    searchQuery.ExactQuery = new CatalogQueryExact("category_id", categoryID);
        //    request.Query = searchQuery;
        //    return request;
        //}

        //public async Task<List<SquareCategory>> GetCategoriesAsync()
        //{
        //    var response = await CatalogCategoriesAsync();
        //    List<SquareCategory> result = new List<SquareCategory>();

        //    foreach(CatalogObject o in response)
        //    {
        //        result.Add(SquareCategory.Load(o));
        //    }

        //    return result;
        //}

        public List<SquareCategory> GetCategories()
        {
            var response = CatalogCategories();
            List<SquareCategory> result = new List<SquareCategory>();

            foreach (CatalogObject o in response)
            {
                result.Add(SquareCategory.Load(o));
            }

            return result;
        }
    }
}
