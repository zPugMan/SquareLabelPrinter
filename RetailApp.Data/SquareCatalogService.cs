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
            if (environment.Equals("Sandbox", StringComparison.CurrentCultureIgnoreCase))
                BaseURL = "https://connect.squareupsandbox.com";
            else if (environment.Equals("Production", StringComparison.CurrentCultureIgnoreCase))
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

        /// <summary>
        /// Retrieve Listing of Catalog categories from Square
        /// </summary>
        /// <returns></returns>
        public List<CatalogObject> CatalogCategories()
        {
            try
            {
                var response = catalogAPI.ListCatalog(null, CatalogObjectType.CATEGORY.ToString());
                if (response.Objects == null)
                {
                    return new List<CatalogObject>();
                }

                return response.Objects;
            } catch(Exception e)
            {
                return new List<CatalogObject>();
            }
            
        }

        /// <summary>
        /// Retrieves Items in Square Catalog for the given Catalog Category, with each variation returned as a separate SquareProduct
        /// </summary>
        /// <param name="category">Catalog Category name</param>
        /// <returns></returns>
        public List<SquareProduct> GetCatalogItemsByCategory(string category)
        {
            SearchCatalogObjectsRequest req = SearchQuery("name", category, SearchCatalogObjectsRequest.ObjectTypesEnum.CATEGORY);

            string categoryID = string.Empty;
            List<SquareProduct> result = new List<SquareProduct>();

            try
            {
                var response = catalogAPI.SearchCatalogObjects(req);


                if (response != null && response.Objects != null)
                {
                    categoryID = response.Objects[0].Id;
                    req = SearchQuery("category_id", categoryID, SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM);

                    var catalogItemsResponse = catalogAPI.SearchCatalogObjects(req);
                    if (catalogItemsResponse != null && catalogItemsResponse.Objects.Count > 0)
                    {

                        foreach (CatalogObject o in catalogItemsResponse.Objects)
                        {
                            result.AddRange(SquareProduct.LoadCatalogObject(o));
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<List<SquareProduct>> GetCatalogItemsByCategoryAsync(string category)
        {
            SearchCatalogObjectsRequest req = SearchQuery("name", category, SearchCatalogObjectsRequest.ObjectTypesEnum.CATEGORY);
            string categoryID = string.Empty;
            List<SquareProduct> result = new List<SquareProduct>();

            try
            {
                var response = await catalogAPI.SearchCatalogObjectsAsyncWithHttpInfo(req);

                if (response == null)
                    return result;

                if (response.StatusCode != 200 || response.Data.Objects == null)
                    return result;

                categoryID = response.Data.Objects[0].Id;
                req = SearchQuery("category_id", categoryID, SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM);
                var catalogItemsResponse = await catalogAPI.SearchCatalogObjectsAsyncWithHttpInfo(req);
                if (catalogItemsResponse != null && catalogItemsResponse.Data.Objects != null)
                {
                    foreach (CatalogObject o in catalogItemsResponse.Data.Objects)
                    {
                        result.AddRange(SquareProduct.LoadCatalogObject(o));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        private SearchCatalogObjectsRequest SearchQuery(string attribute, string categoryName, SearchCatalogObjectsRequest.ObjectTypesEnum squareObjectType)
        {
            SearchCatalogObjectsRequest req = new SearchCatalogObjectsRequest();
            req.IncludeRelatedObjects = true;
            req.ObjectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum>() { squareObjectType };
            req.Query = new CatalogQuery();
            req.Query.ExactQuery = new CatalogQueryExact(AttributeName: attribute, AttributeValue: categoryName);
            return req;
        }

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

        public async Task<List<SquareCategory>> GetCategoriesAsync()
        {
            var response = await CatalogCategoriesAsync();
            List<SquareCategory> result = new List<SquareCategory>();

            foreach (CatalogObject o in response)
            {
                result.Add(SquareCategory.Load(o));
            }

            return result;
        }
    }
}
