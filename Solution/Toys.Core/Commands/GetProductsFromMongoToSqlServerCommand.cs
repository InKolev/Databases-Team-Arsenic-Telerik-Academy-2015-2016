namespace Toys.Core.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class GetProductsFromMongoToSqlServerCommand : Command, ICommand
    {
        private const string Arsenicdbinmongodb = "ArsenicDbInMongoDb";
        private const string ProductsTable = "Products";

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

            if (this.Data.Products.All().Any() || !docs.Any())
            {
                return false;
            }

            this.SaveProductsToSqlServer(docs);

            return true;
        }

        private async Task<List<BsonDocument>> LoadProductsFromMongoDb()
        {
            var collection = this.mongoDatabase.GetCollection<BsonDocument>(ProductsTable);
            var dataFromMongo = await collection.Find(x => true).ToListAsync();

            return dataFromMongo;
        }

        private void SaveProductsToSqlServer(IEnumerable<BsonDocument> documents)
        {
            var product = new Product();
            string sku;

            foreach (var document in documents)
            {
                sku = document["Sku"].ToString();

                // Skip duplicate Sku values, because by design Sku is unique and Ef exploed with exception
                if (this.Data.Products.All().Any(x => x.Sku.Equals(sku)))
                {
                    continue;
                }

                product.Sku = sku;
                product.Description = document["Description"].ToString();
                product.WholesalePrice = decimal.Parse(document["WholesalePrice"].ToString());
                product.RetailPrice = decimal.Parse(document["RetailPrice"].ToString());
                product.TradeDiscount = decimal.Parse(document["TradeDiscount"].ToString());
                product.TradeDiscountRate = float.Parse(document["TradeDiscountRate"].ToString());
                product.ManufacturerId = int.Parse(document["ManufacturerId"].ToString());

                this.Data.Products.Add(product);
                this.Data.SaveChanges();
            }
        }
    }
}