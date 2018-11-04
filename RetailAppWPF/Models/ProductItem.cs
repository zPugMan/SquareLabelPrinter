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
    }
}
