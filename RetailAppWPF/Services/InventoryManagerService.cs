using RetailApp.Data;
using RetailAppWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppWPF.Services
{
    public class InventoryManagerService
    {
        private const string SUCCESS_RESULT = "Successful Inventory Update";

        public (bool IsSuccess, string Message) AddInventory(ProductItem product, int addQuantity)
        {
            var svc = new InventoryService(
                location: Properties.Settings.Default.Location,
                accessToken: Properties.Settings.Default.AccessToken,
                environment: Properties.Settings.Default.Environment
                );
            var result = svc.AddInventory(product.VariationId, addQuantity);

            return ParseApiResponse(result);
        }

        private (bool IsSuccess, string Message) ParseApiResponse(ApiResponse response)
        {
            if (response.IsSuccess)
                return (response.IsSuccess, SUCCESS_RESULT);
            else
                return (response.IsSuccess, response.Errors[0].Message);
        }
    }
}
