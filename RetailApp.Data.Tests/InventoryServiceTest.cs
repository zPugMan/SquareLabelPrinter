using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Square.Connect.Api;
using Square.Connect.Model;

namespace RetailApp.Data.Tests
{
    [TestClass]
    public class InventoryServiceTest
    {
        //[TestMethod]
        //public void TestGetInventory()
        //{
        //    SquareCatalogService svc = new SquareCatalogService();
        //    List<CatalogObject> categories = svc.CatalogCategories();

        //    CatalogObject co = categories.Find(x => x.CategoryData.Name == "Other");

        //    List<CatalogObject> items = svc.CatalogItems(co.Id);
        //    CatalogObject item = items.Find(x => x.ItemData.Name == "Test Beauty");

        //    InventoryService iSvc = new InventoryService();

        //    InventoryCount itemCount = iSvc.GetInventory(item.ItemData.Variations[0].Id);
        //    Assert.IsNotNull(itemCount);
        //}

        //[TestMethod]
        //public void TestAddInventory()
        //{
        //    SquareCatalogService svc = new SquareCatalogService();
        //    List<CatalogObject> categories = svc.CatalogCategories();

        //    CatalogObject co = categories.Find(x => x.CategoryData.Name == "Other");

        //    List<CatalogObject> items = svc.CatalogItems(co.Id);
        //    CatalogObject item = items.Find(x => x.ItemData.Name == "Test Beauty");

        //    InventoryService iSvc = new InventoryService();
        //    InventoryChange change = new InventoryChange();
        //    change.Type = InventoryChange.TypeEnum.ADJUSTMENT;
        //    InventoryAdjustment adjustment = new InventoryAdjustment();
        //    adjustment.CatalogObjectId = item.ItemData.Variations[0].Id;
        //    adjustment.Quantity = "2";

        //    LocationsApi locoAPI = new LocationsApi();
        //    var respLoco = locoAPI.ListLocations();

        //    adjustment.LocationId = respLoco.Locations[0].Id;
        //    adjustment.FromState = InventoryAdjustment.FromStateEnum.NONE;
        //    adjustment.ToState = InventoryAdjustment.ToStateEnum.INSTOCK;
        //    adjustment.OccurredAt = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ssZ");

        //    change.Adjustment = adjustment;
            

        //    iSvc.AddInventory(Guid.NewGuid(), change);
        //}


    }
}
