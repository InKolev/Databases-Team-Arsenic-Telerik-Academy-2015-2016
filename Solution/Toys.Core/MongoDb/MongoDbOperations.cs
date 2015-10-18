namespace Toys.Core.MongoDb
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;


    public class MongoDbOperations
    {
        private IMongoClient client;
        private IMongoDatabase database;


        public MongoDbOperations()
        {
            this.client = new MongoClient();
            this.database = this.client.GetDatabase("ArsenicDb");

        }

        public void ImportProducts()
        {
            var path = @"../../../Files/DbProductsToImportInMongoDb.txt";

            using (StreamReader reader = new StreamReader(path))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    var data = line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    this.SaveProductsToDb(data);

                    line = reader.ReadLine();

                    Thread.Sleep(50);
                    Console.Write('.');
                }
            }
        }

        public async void LoadProducts()
        {
            //var collection = this.database.GetCollection<BsonDocument>("Products");
            //var filter = new BsonDocument();
            //var count = 0;

            //using (var cursor = collection.Find(filter))
            //{
            //    while (cursor.ToJson())
            //    {
            //        var batch = cursor.Current;
            //        count += batch.Count();
            //    }

            //    Thread.Sleep(5000);
            //    Console.WriteLine(count);
            //}
        }

        private async void SaveProductsToDb(string[] data)
        {
            var document = new BsonDocument
            {
                {"Id", data[0]},
                {"Sku", data[1]},
                {"ManufacturerId", data[2]},
                {"Description", data[3]},
                {"WholesalePrice", new BsonDouble(double.Parse(data[4])) },
                {"RetailPrice", new BsonDouble(double.Parse(data[5])) },
                {"TradeDiscount", new BsonDouble(double.Parse(data[6])) },
                {"TradeDiscountRate", new BsonDouble(double.Parse(data[7])) }
            };

            var collection = database.GetCollection<BsonDocument>("Products");
            await collection.InsertOneAsync(document);
        }
    }
}
