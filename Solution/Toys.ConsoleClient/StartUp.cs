namespace Toys.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Threading;
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

            var mongo = new MongoDbOperations(db);

            //mongo.ImportProducts();
            var data = mongo.LoadProductsFromMongoDb().Result;

            mongo.SaveProductsToSqlServer(data);

            //mongo.Play().Wait();
        }
    }
}