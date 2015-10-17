namespace Toys.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class ToysData : IToysData
    {
        private readonly DbContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public ToysData(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context
        {
            get { return this.context; }
        }

        public IRepository<Product> Products
        {
            get { return this.GetRepository<Product>(); }
        }

        public IRepository<Manufacturer> Manufacturers
        {
            get { return this.GetRepository<Manufacturer>(); }
        }

        public IRepository<Country> Countries
        {
            get { return this.GetRepository<Country>(); }
        }

        public IRepository<Sale> Sales
        {
            get { return this.GetRepository<Sale>(); }
        }

        public IRepository<Saler> Salers
        {
            get { return this.GetRepository<Saler>(); }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.context != null)
            {
                this.context.Dispose();
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (this.repositories.ContainsKey(typeof (T)))
            {
                return (IRepository<T>) this.repositories[typeof (T)];
            }

            var type = typeof(Repository<T>);
            this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
