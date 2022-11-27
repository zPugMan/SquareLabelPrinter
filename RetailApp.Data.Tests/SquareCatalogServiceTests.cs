using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Square.Connect.Api;
using Square.Connect.Model;
using Moq;
using Square.Connect.Client;

namespace RetailApp.Data.Tests
{
    [TestClass()]
    public class SquareCatalogServiceTests
    {

        [TestMethod()]
        public void SquareCatalogServiceTest_InvalidEnvironment()
        {
            Assert.ThrowsException<Exception>(()=>new SquareCatalogService("invalid location", "123123", "invalid environment"));
        }

        [TestMethod()]
        public void SquareCatalogServiceTest_SandboxInit()
        {
            SquareCatalogService svc = new SquareCatalogService("test location", "123123", "SandboX");

            Assert.IsNotNull(svc);
        }

        [TestMethod()]
        public void SquareCatalogServiceTest_ProductionInit()
        {
            SquareCatalogService svc = new SquareCatalogService("test location", "123123", "ProDucTion");

            Assert.IsNotNull(svc);
        }

        [TestMethod()]
        public void CatalogCategoriesAsyncTest()
        {
            //arrange
            List<CatalogObject> mockCatalogItems = TestCatalogObjects();

            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.ListCatalogAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new ListCatalogResponse(null, null, mockCatalogItems));

            SquareCatalogService svc = new SquareCatalogService(mock.Object);

            //act
            var response = svc.CatalogCategoriesAsync();

            //assert
            Assert.IsTrue(response.Result != null, $"Expected result, but received null");

        }

        private List<CatalogObject> TestCatalogObjects()
        {
            List<CatalogObject> mockCatalogItems = new List<CatalogObject>();
            mockCatalogItems.Add(new CatalogObject(
                Type: CatalogObject.TypeEnum.CATEGORY,
                Id: "123",
                CategoryData: new CatalogCategory() { Name = "Alchemic" }
                )
            );

            mockCatalogItems.Add(new CatalogObject(
                Type: CatalogObject.TypeEnum.CATEGORY,
                Id: "333",
                CategoryData: new CatalogCategory() { Name = "Essentials" }
                )
            );

            return mockCatalogItems;
        }

        private Mock<ICatalogApi> TestListCatalogResults()
        {
            List<CatalogObject> mockCatalogItems = TestCatalogObjects();

            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.ListCatalog(
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => new ListCatalogResponse(null, null, mockCatalogItems));

            return mock;
        }

        [TestMethod()]
        public void CatalogCategoriesTest_OK()
        {
            //arrange
            var mock = TestListCatalogResults();
            SquareCatalogService svc = new SquareCatalogService(mock.Object);

            //act
            var result = svc.CatalogCategories();

            //assert
            Assert.IsNotNull(result.Count, $"Expecting 2 catalog entries, received: {0}", result.Count);

        }

        [TestMethod()]
        public void CatalogCategoriesTest_ExceptionFail()
        {
            //arrange
            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.ListCatalog(
                    It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>(() => new Exception("Something evil happened.. throw throw"));
            SquareCatalogService svc = new SquareCatalogService(mock.Object);

            //act
            var result = svc.CatalogCategories();

            //assert
            Assert.IsTrue(result.GetType() == typeof(List<CatalogObject>), $"Expecting empty list result; found {result.GetType().Name}");
            Assert.IsTrue(result.Count == 0, $"Return result expected 0; found {result.Count}");
        }

        [TestMethod()]
        public void CatalogCategoriesTest_Errors()
        {
            //arrange
            List<Error> errors = new List<Error>();
            errors.Add(new Error(Category: Error.CategoryEnum.AUTHENTICATIONERROR, Code: Error.CodeEnum.UNAUTHORIZED, Detail: "Non-Authorized", Field: null));

            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.ListCatalog(
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => new ListCatalogResponse(Errors: errors, Cursor: null, null));

            SquareCatalogService svc = new SquareCatalogService(mock.Object);

            //act
            var result = svc.CatalogCategories();

            //assert
            Assert.IsTrue(result.GetType() == typeof(List<CatalogObject>), $"Expecting empty list result, found {result.GetType().Name}");
            Assert.IsTrue(result.Count == 0, $"Return result expected 0; found {result.Count}");
        }

        [TestMethod()]
        public void GetCatelogItemsByCategoryTest_Errors()
        {
            //arrange
            List<Error> errors = new List<Error>();
            errors.Add(new Error(Error.CategoryEnum.INVALIDREQUESTERROR, Code: Error.CodeEnum.INTERNALSERVERERROR, Detail: "Big problems", null));

            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.SearchCatalogObjects(It.IsAny<SearchCatalogObjectsRequest>()))
               .Returns(() => new SearchCatalogObjectsResponse(Errors: errors, null, null, null));

            SquareCatalogService svc = new SquareCatalogService(mock.Object);

            //act
            var result = svc.GetCatalogItemsByCategory(string.Empty);

            //assert
            Assert.IsTrue(result.GetType() == typeof(List<SquareProduct>), $"Expecting response type of List<{nameof(SquareProduct)}>, received {result.GetType().Name}");
            Assert.IsTrue(result.Count == 0, $"Expecting empty List<{nameof(SquareProduct)}>, found count: {result.Count}");
        }

        [TestMethod()]
        public void GetCatalogItemsByCategoryTest_OK()
        {
            //arrange
            List<CatalogObject> itemTypes = new List<CatalogObject>();
            itemTypes.Add(
                new CatalogObject(CatalogObject.TypeEnum.ITEMVARIATION, Id: "AB-2333-A1", 
                    ItemVariationData: new CatalogItemVariation(ItemId: "AB-2333-A", Sku: "SKU-2322", Name: "Small",
                        PricingType: CatalogItemVariation.PricingTypeEnum.FIXEDPRICING, 
                        PriceMoney: new Money(3500, Money.CurrencyEnum.USD))
                    ));
            itemTypes.Add(new CatalogObject(CatalogObject.TypeEnum.ITEMVARIATION, Id: "AB-2333-A2",
                    ItemVariationData: new CatalogItemVariation(ItemId: "AB-2333-B", Sku: "SKU-232L", Name: "Large",
                        PricingType: CatalogItemVariation.PricingTypeEnum.FIXEDPRICING,
                        PriceMoney: new Money(7500, Money.CurrencyEnum.USD))
                    ));
            CatalogItem item = new CatalogItem(Name: "DEDE", "Shampoo stuff", Variations: itemTypes, ProductType: CatalogItem.ProductTypeEnum.RETAILITEM);
            List<CatalogObject> catalogObjects = new List<CatalogObject>();
            catalogObjects.Add(new CatalogObject(Type: CatalogObject.TypeEnum.ITEM, Id: "AB-2333", ItemData: item));
            SearchCatalogObjectsResponse searchResponse = new SearchCatalogObjectsResponse(Errors: null, Cursor: null, Objects: catalogObjects, RelatedObjects: null);

            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.SearchCatalogObjects(It.IsAny<SearchCatalogObjectsRequest>()))
                .Returns(() => searchResponse);

            SquareCatalogService svc = new SquareCatalogService(mock.Object);

            //act
            var result = svc.GetCatalogItemsByCategory("Category1");

            //assert
            Assert.IsTrue(result.GetType() == typeof(List<SquareProduct>), $"Expecting response type List<${nameof(SquareProduct)}>, received: {result.GetType().Name}");
            Assert.IsTrue(result.Count == itemTypes.Count, $"Expecting result of 1, received {result.Count}");
        }

        [TestMethod()]
        public void GetCatalogItemsByCategoryTest_Fail()
        {
            //arrange
            var mock = new Mock<ICatalogApi>();
            mock.Setup(m => m.SearchCatalogObjects(It.IsAny<SearchCatalogObjectsRequest>()))
                .Throws(new Exception("Error, error.. what is the result?"));

            var svc = new SquareCatalogService(mock.Object);

            //act
            var result = svc.GetCatalogItemsByCategory("Category1");

            //assert
            Assert.IsTrue(result.GetType() == typeof(List<SquareProduct>), $"Expecting response type List<${nameof(SquareProduct)}>, received: {result.GetType().Name}");
            Assert.IsTrue(result.Count == 0, $"Expecting result of 1, received {result.Count}");
        }

        [TestMethod()]
        public void GetCategoriesTest()
        {
            //arrange 
            var mock = TestListCatalogResults();
            SquareCatalogService service = new SquareCatalogService(mock.Object);

            //act
            var result = service.GetCategories();

            //assert
            Assert.IsTrue(result.GetType() == typeof(List<SquareCategory>), $"Return type of {result.GetType().Name} does not match expected return type");
            Assert.IsTrue(result.Count == 2, $"Expected return count of 2, received: {result.Count}");
        }
    }
}