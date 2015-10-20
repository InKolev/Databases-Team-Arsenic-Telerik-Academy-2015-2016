namespace Toys.Data
{
    using System.Data.Entity;
    using Toys.Models;

    public class ToysSqliteContext : DbContext
    {
        public ToysSqliteContext()
            : base("SqliteContext")
        {
        }

        public virtual IDbSet<Product> Products { get; set; }
    }
}