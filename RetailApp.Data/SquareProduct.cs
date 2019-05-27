using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data
{
    public class SquareProduct
    {
        public SquareProduct() { }

        public SquareCategory Category { get; set; }
        public string VariationId { get; set; }
        public string VariationName { get; set; }
        public string SKU { get; set; }
        public long? Price { get; set; }
        public string ItemName { get; set; }

        /// <summary>
        /// Helper. Converter
        /// </summary>
        /// <param name="catalogObject"></param>
        /// <returns></returns>
        public static List<SquareProduct> LoadCatalogObject(CatalogObject catalogObject)
        {
            List<SquareProduct> products = new List<SquareProduct>();

            SquareProduct r = null;

            for(int i=0; i<catalogObject.ItemData.Variations.Count; i++)
            {
                r = new SquareProduct();
                r.ItemName = catalogObject.ItemData.Name;
                r.Price = catalogObject.ItemData.Variations[i].ItemVariationData.PriceMoney.Amount;
                r.SKU = catalogObject.ItemData.Variations[i].ItemVariationData.Sku;
                r.VariationName = catalogObject.ItemData.Variations[i].ItemVariationData.Name;
                r.VariationId = catalogObject.ItemData.Variations[i].Id;
                products.Add(r);
            }
            return products;
        }
    }
}
