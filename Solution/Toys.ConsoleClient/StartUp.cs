namespace Toys.ConsoleClient
{
    using System;
    using System.Linq;
    using Toys.Core.MongoDb;
    using Toys.Data;

    public class StartUp
    {
        private static void Main()
        {
            // With this code you could thest successful creation of db
            var context = new ToysDbContext();
            var db = new ToysData(context);

            Console.WriteLine(db.Products.All().Count());

            var mongo = new MongoDbOperations();

            // mongo.ImportProducts();
            mongo.LoadProducts();
        }
    }
}
