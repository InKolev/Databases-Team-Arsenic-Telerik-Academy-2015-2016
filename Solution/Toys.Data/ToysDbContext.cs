namespace Toys.Data
{
    using System.Data.Entity;
    using Toys.Data.Migrations;
    using Toys.Models;

    public class ToysDbContext : DbContext
    {
        public ToysDbContext()
            : base("ArsenicDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ToysDbContext, Configuration>());
        }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<Manufacturer> Manufacturers { get; set; }

        public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<Sale> Sales { get; set; }

        public virtual IDbSet<Seller> Sellers { get; set; }
    }
}
