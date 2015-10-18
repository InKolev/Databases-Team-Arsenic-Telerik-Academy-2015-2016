namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class GetProductsFromMongoToSqlServerCommand : Command, ICommand
    {
        private const string Arsenicdbinmongodb = "ArsenicDbInMongoDb";

        private readonly IMongoClient mongoClient = new MongoClient();
        private readonly IMongoDatabase mongoDatabase;

        public GetProductsFromMongoToSqlServerCommand(IToysData data)
            : base(data)
        {
            this.mongoDatabase = this.mongoClient.GetDatabase(Arsenicdbinmongodb);
        }

        public override bool Execute()
        {
            var docs = this.LoadProductsFromMongoDb().Result;

            return true;
        }

        private async Task<List<BsonDocument>> LoadProductsFromMongoDb()
        {
            var collection = this.mongoDatabase.GetCollection<BsonDocument>("Products");
            var dataFromMongo = await collection.Find(x => true).ToListAsync();

            return dataFromMongo;
        }

        private void SaveProductsToSqlServer(List<BsonDocument> documents)
        {
            var product = new Product();

            foreach (var document in documents)
            {
                product.Sku = document["Sku"].ToString();
                product.Description = document["Description"].ToString();
                product.WholesalePrice = decimal.Parse(document["WholesalePrice"].ToString());
                product.RetailPrice = decimal.Parse(document["RetailPrice"].ToString());
                product.TradeDiscount = decimal.Parse(document["TradeDiscount"].ToString());
                product.TradeDiscountRate = float.Parse(document["TradeDiscountRate"].ToString());
                product.ManufacturerId = int.Parse(document["ManufacturerId"].ToString());

                this.Data.Products.Add(product);

                try
                {
                    this.Data.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            // var product = new Product
            // {
            // };
            // this.sqlServerDb.Products.Add(product);
            // this.sqlServerDb.SaveChanges();
        }
    }
}