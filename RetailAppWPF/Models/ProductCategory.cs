using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailAppWPF.Models
{
    public class ProductCategory
    {
        public string Name { get; set; }
        public IEnumerable<ProductItem> Products { get; set; }
        public int ProductCount => Products.Count();
    }
}
