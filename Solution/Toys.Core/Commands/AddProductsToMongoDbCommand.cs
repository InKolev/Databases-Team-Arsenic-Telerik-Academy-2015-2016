﻿namespace Toys.Core.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Toys.Data.Contracts;

    public class AddProductsToMongoDbCommand : Command, ICommand
    {
        private const string ProductsTextFilePaht = @"../../../Files/DbProductsToImportInMongoDb.txt";
        private const string Arsenicdbinmongodb = "ArsenicDbInMongoDb";

        private readonly IMongoClient mongoClient = new MongoClient();
        private readonly IMongoDatabase mongoDatabase;

        public AddProductsToMongoDbCommand(IToysData data)
            : base(data)
        {
            this.mongoDatabase = this.mongoClient.GetDatabase(Arsenicdbinmongodb);
        }

        public override bool Execute()
        {
            var dataToImport = this.ImportFromTextFile(ProductsTextFilePaht);

            if (!dataToImport.Any())
            {
                return false;
            }

            foreach (var data in dataToImport)
            {
                this.SaveProductsToDb(data).Wait();
            }

            return true;
        }

        private async Task SaveProductsToDb(string[] data)
        {
            var document = new BsonDocument
            {
                { "Id", data[0] },
                { "Sku", data[1] },
                { "ManufacturerId", data[2] },
                { "Description", data[3] },
                { "WholesalePrice", new BsonDouble(double.Parse(data[4])) },
                { "RetailPrice", new BsonDouble(double.Parse(data[5])) },
                { "TradeDiscount", new BsonDouble(double.Parse(data[6])) },
                { "TradeDiscountRate", new BsonDouble(double.Parse(data[7])) }
            };

            var collection = this.mongoDatabase.GetCollection<BsonDocument>("Products");
            await collection.InsertOneAsync(document);
        }
    }
}