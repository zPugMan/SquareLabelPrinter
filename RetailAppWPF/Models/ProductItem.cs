using RetailApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppWPF.Models
{
    public class ProductItem
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string VariationId { get; set; }
        private string price;
        public string Price
        {
            get
            {
                if (price.Length > 4)
                {
                    var pPrice = price.Split('.');
                    return String.Format("${0}",pPrice[0]);
                }
                else
                    return String.Format("${0}", price);
            }
            set { price = value; }
        }

        public static List<ProductItem> ToList(List<SquareProduct> p)
        {
            List<ProductItem> result = new List<ProductItem>();
            foreach(SquareProduct prod in p)
            {
                ProductItem item = new ProductItem();
                item.Name = prod.ItemName;
                item.SKU = prod.SKU;
                item.Size = prod.VariationName;
                item.VariationId = prod.VariationId;
                item.price = String.Format("{0:#.00}", Convert.ToDecimal(prod.Price) / 100);

                if(!result.Exists(x=> x.SKU == item.SKU))
                    result.Add(item);
            }
            return result;
        }
    }
}
