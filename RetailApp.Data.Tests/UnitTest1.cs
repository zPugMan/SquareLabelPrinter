using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailApp.Data;
using Square.Connect.Model;

namespace RetailApp.Data.Tests
{
    [TestClass]
    public class CatalogServiceTest
    {
        [TestMethod]
        public async Task TestCatalogCategoriesAsync()
        {
            SquareCatalogService svc = new SquareCatalogService();
            var response = await svc.CatalogCategoriesAsync();
            CollectionAssert.AllItemsAreInstancesOfType(response, typeof(CatalogCategory), "Collection is not typeof Square.Connect.Model.CatalogCategory");
        }

        [TestMethod]
        public void TestCatalogItems()
        {
            SquareCatalogService svc = new SquareCatalogService();
            List<CatalogObject> categories = svc.CatalogCategories();
            CatalogObject category = categories.Find(x => x.CategoryData.Name == "Alchemic");
            var response = svc.CatalogItems(category.Id);
        }

        [TestMethod]
        public void TestCatalogItemsById()
        {
            SquareCatalogService svc = new SquareCatalogService();
            List<CatalogObject> categories = svc.CatalogCategories();
            CatalogObject cat = categories[0];
            List<CatalogObject> items = svc.CatalogItems(cat.Id);
        }

        [TestMethod]
        public void TestCatalogItemsByName()
        {
            SquareCatalogService svc = new SquareCatalogService();
            var response = svc.GetCatelogItemsByCategory("Alchemic");
        }
    }
}
