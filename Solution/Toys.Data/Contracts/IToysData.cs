namespace Toys.Data.Contracts
{
    using System.Data.Entity;
    using Toys.Models;

    public interface IToysData
    {
        DbContext Context { get; }

        IRepository<Product> Products { get; }

        IRepository<Manufacturer> Manufacturers { get; }

        IRepository<Country> Countries { get; }

        IRepository<Sale> Sales { get; }

        IRepository<Saler> Salers { get; }

        void Dispose();

        int SaveChanges();
    }
}