using Square.Connect.Api;
using Square.Connect.Client;
using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data
{
    public class InventoryService
    {
        private IInventoryApi inventoryAPI;
        private readonly string Location = ConfigurationManager.AppSettings.Get("squareLocation");
        private readonly string BaseURL = ConfigurationManager.AppSettings.Get("squareClientUrl");
        private readonly string AccessToken = ConfigurationManager.AppSettings.Get("squareAccessToken");

        public InventoryService()
        {
            //Configuration.Default.AccessToken = "EAAAEOuEbej1xTRYe87QtBrxFosUNbeyF3Cn15PcNFFCltL-6_LbT-0mr2AL7zha";  //TODO refactor this "sq0atp-1HHSi3YYpmrFm59ANOiiPw";
            var config = new Square.Connect.Client.Configuration(new ApiClient(basePath: BaseURL));
            config.AccessToken = AccessToken;
            inventoryAPI = new InventoryApi(config);
        }

        public InventoryService(IInventoryApi api)
        {
            inventoryAPI = api;
        }

        public InventoryCount GetInventory(String catalogObjectId)
        {
            try
            {
                var response = inventoryAPI.RetrieveInventoryCountWithHttpInfo(catalogObjectId, null, null);
                if (response.Data.Counts == null)
                    return null;

                return response.Data.Counts.First();
            } catch(ApiException e)
            {
                return null;
            } catch(Exception e)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Adds to existing inventory for an item
        /// </summary>
        /// <param name="catalogObjectId"><see cref="Square.Connect.Model.InventoryAdjustment.CatalogObjectId"/> which corresponds to <see cref="Square.Connect.Model.CatalogItemVariation.ItemId"/></param>
        /// <param name="changeQuantity">positve value to add to the existing inventory</param>
        /// <returns></returns>
        public ApiResponse AddInventory(string catalogObjectId, int changeQuantity)
        {
            if (changeQuantity < 1)
                return new ApiResponse("Invalid amount to add inventory", false);

            var itemAdjustment = new InventoryAdjustment(
                null
                , null
                , InventoryAdjustment.FromStateEnum.NONE
                , InventoryAdjustment.ToStateEnum.INSTOCK
                , LocationId: Location
                , CatalogObjectId: catalogObjectId
                , CatalogObjectType: null
                , Quantity: changeQuantity.ToString()
                , OccurredAt: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                );

            var itemChange = new InventoryChange(InventoryChange.TypeEnum.ADJUSTMENT
                , PhysicalCount: null
                , Adjustment: itemAdjustment
                , Transfer: null);

            BatchChangeInventoryRequest req = new BatchChangeInventoryRequest();
            req.IdempotencyKey = Guid.NewGuid().ToString();
            req.Changes = new List<InventoryChange>() { itemChange };

            try
            {
                var response = inventoryAPI.BatchChangeInventoryWithHttpInfo(req);
                if(response != null && response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return new ApiResponse(response.Data.Errors, false);
                }

                return new ApiResponse(response.Data.Errors, true);
            } catch(ApiException e)
            {
                return new ApiResponse(e.Message, false);
            } catch(Exception e)
            {
                return new ApiResponse(e.Message, false);
            }
        }
    }
}
