using Square.Connect.Api;
using Square.Connect.Client;
using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data
{
    public class InventoryService
    {
        private IInventoryApi inventoryAPI;
        internal const string SANDBOX = "EAAAEOuEbej1xTRYe87QtBrxFosUNbeyF3Cn15PcNFFCltL-6_LbT-0mr2AL7zha";
        internal const string PROD = "***REMOVED***";
        internal const string LOCATION = "LEASAZNP0VFC5";   //SANDBOX location

        public InventoryService()
        {
            //Configuration.Default.AccessToken = "EAAAEOuEbej1xTRYe87QtBrxFosUNbeyF3Cn15PcNFFCltL-6_LbT-0mr2AL7zha";  //TODO refactor this "***REMOVED***";
            //Configuration.Default.AccessToken = SANDBOX;
            var config = new Configuration(new ApiClient(basePath: "https://connect.squareupsandbox.com"));
            config.AccessToken = SANDBOX;
            inventoryAPI = new InventoryApi(config);
        }

        public InventoryService(IInventoryApi api)
        {
            inventoryAPI = api;
            //inventoryAPI.Configuration.AccessToken = SANDBOX;
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

        //public async Task<InventoryCount> GetInventoryAsync(String id)
        //{
        //    var response = await inventoryAPI.RetrieveInventoryCountAsync(id);
        //    return response.Counts.First();
        //}

        public ApiResponse AddInventory(string catalogObjectId, int changeQuantity)
        {
            if (changeQuantity < 1)
                return new ApiResponse("Invalid amount to add inventory", false);

            var itemAdjustment = new InventoryAdjustment(
                null
                , null
                , InventoryAdjustment.FromStateEnum.NONE
                , InventoryAdjustment.ToStateEnum.INSTOCK
                , LocationId: LOCATION
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
