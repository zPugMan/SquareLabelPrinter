using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Square.Connect.Api;
using Square.Connect.Model;
using Moq;
using Square.Connect.Client;

namespace RetailApp.Data.Tests
{
    [TestClass]
    public class InventoryServiceTest
    {
        [TestInitialize]
        public void Setup()
        {
            mockHeader = new Dictionary<string, string>();
            mockHeader.Add("X-Speleo-Traceid", "test123");
        }

        private IDictionary<string, string> mockHeader;

        [TestMethod]
        public void TestGetInventorySuccess()
        {
            //arrange
            var testCatalogOjbectId = "VPIGUXPK7GOZEGFFL7E7F5NA";
            int mockCount = 6;
            var mockInventoryCount = new List<InventoryCount>();
            mockInventoryCount.Add(new InventoryCount(testCatalogOjbectId, "ITEM_VARIATION", InventoryCount.StateEnum.INSTOCK, "LEASAZNP0VFC5", mockCount.ToString(), null));
            var mockResponse = new RetrieveInventoryCountResponse(null, mockInventoryCount, null);
            var mock = new Mock<IInventoryApi>();
            mock.Setup(m => m.RetrieveInventoryCountWithHttpInfo(
                    It.IsAny<string>(), null, null))
                .Returns(() =>
                    new ApiResponse<RetrieveInventoryCountResponse>(200, mockHeader, mockResponse));

            //act
            var svc = new InventoryService(mock.Object);
            var result = svc.GetInventory(testCatalogOjbectId);

            //assert
            Assert.IsTrue(int.Parse(result.Quantity) == mockCount, "Returned response for inventory count does not match expected value");
        }

        [TestMethod]
        public void TestGetInventoryFail()
        {
            //arrange
            var testCatalogObjectId = "blah-blah";
            var mock = new Mock<IInventoryApi>();
            mock.Setup(m => m.RetrieveInventoryCountWithHttpInfo(
                    It.IsAny<string>(), null, null))
                .Throws<ApiException>(() => new ApiException(401, "Unauthorized"));

            //act
            var svc = new InventoryService(mock.Object);
            var result = svc.GetInventory(testCatalogObjectId);

            //assert
            Assert.IsTrue(result == null, "Expecting null response on ApiException");
        }

        [TestMethod]
        public void TestAddInventorySuccess()
        {
            var addQuantity = 5;
            var catalogObject = "blah-blah";

            //arrange
            var countItemResponse = new InventoryCount(CatalogObjectId: catalogObject, CatalogObjectType: CatalogObjectType.ITEMVARIATION.ToString()
                            , State: InventoryCount.StateEnum.INSTOCK, LocationId: "LocationX", Quantity: addQuantity.ToString());
            var changeResponse = new BatchChangeInventoryResponse(
                    Errors: null
                    , Counts: new List<InventoryCount>() { countItemResponse }
                );

            var mock = new Mock<IInventoryApi>();
            mock.Setup(m => m.BatchChangeInventoryWithHttpInfo(
                    It.IsAny<BatchChangeInventoryRequest>()))
                .Returns(() =>
                    new ApiResponse<BatchChangeInventoryResponse>(200, mockHeader, changeResponse));

            //act
            var svc = new InventoryService(mock.Object);
            var result = svc.AddInventory(catalogObject, addQuantity);

            //assert
            Assert.IsTrue(result.IsSuccess, "Expecting successful add response");
            Assert.IsTrue(result.Errors == null, "Expecting no errors in resulting error list");
        }

        [TestMethod]
        public void TestAddInventoryFail()
        {
            //arrange
            var mock = new Mock<IInventoryApi>();
            mock.Setup(m => m.BatchChangeInventoryWithHttpInfo(
                    It.IsAny<BatchChangeInventoryRequest>()))
                .Throws<ApiException>(() => new ApiException(401, "Unauthorized"));

            //act
            var svc = new InventoryService(mock.Object);
            var result = svc.AddInventory("blah-blah", 5);

            //assert
            Assert.IsTrue(!result.IsSuccess, "Expecting non-success response on ApiException");
        }

        [TestMethod]
        public void TestAddInventoryInvalidAmount()
        {
            //arrange
            //act
            var svc = new InventoryService();
            var result = svc.AddInventory("blah-blah", -4);

            //assert
            Assert.IsTrue(!result.IsSuccess, "Negative amount should result in non-success");
            Assert.IsTrue(result.Errors != null && result.Errors.Count > 0, "Expecting at least one error in response");
        }

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
