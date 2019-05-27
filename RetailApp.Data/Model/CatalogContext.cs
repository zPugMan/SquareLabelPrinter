using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailApp.Data.Model
{
    public class CatalogContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }

        public CatalogContext() : base("CatalogContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
