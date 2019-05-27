using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data.Model
{
    public class CatalogItem
    {
        public string SquareId { get; set; }
        public string SquareName { get; set; }
        public string VariationId { get; set; }
        public string VariationName { get; set; }
        public string VariationSKU { get; set; }
        public double VariationPrice { get; set; }
    }
}
