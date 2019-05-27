using Square.Connect.Api;
using Square.Connect.Client;
using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data
{
    public class InventoryService
    {
        private InventoryApi inventoryAPI;

        public InventoryService()
        {
            Configuration.Default.AccessToken = "sq0atp-1HHSi3YYpmrFm59ANOiiPw";
            inventoryAPI = new InventoryApi();
        }

        public InventoryCount GetInventory(String id)
        {
            var response = inventoryAPI.RetrieveInventoryCount(id);
            return response.Counts.First();
        }

        public async Task<InventoryCount> GetInventoryAsync(String id)
        {
            var response = await inventoryAPI.RetrieveInventoryCountAsync(id);
            return response.Counts.First();
        }

        //TODO inventory update
        public void AddInventory(Guid guid, InventoryChange change)
        {
            List<InventoryChange> changes = new List<InventoryChange>();
            changes.Add(change);

            BatchChangeInventoryRequest req = new BatchChangeInventoryRequest();
            req.IdempotencyKey = guid.ToString();
            req.Changes = changes;
            var response = inventoryAPI.BatchChangeInventory(req);
        }
    }
}
