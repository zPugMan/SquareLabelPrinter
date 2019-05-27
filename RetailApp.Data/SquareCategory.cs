using Square.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data
{
    public class SquareCategory
    {
        public static string UNKNOWN_CATEGORY = "Unknown";
        public SquareCategory() { }
        public string Id { get; set; }
        public string CategoryName { get; set; }

        public static SquareCategory Load(CatalogObject obj)
        {
            SquareCategory cat = new SquareCategory();
            cat.Id = obj.Id;
            cat.CategoryName = obj.CategoryData != null ? obj.CategoryData.Name : UNKNOWN_CATEGORY;

            return cat;
        }
    }
}
