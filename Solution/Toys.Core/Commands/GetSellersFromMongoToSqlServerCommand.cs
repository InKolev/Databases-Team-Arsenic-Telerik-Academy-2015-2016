namespace Toys.Core.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class GetSellersFromMongoToSqlServerCommand : Command, ICommand
    {
        private const string Arsenicdbinmongodb = "ArsenicDbInMongoDb";
        private const string SellersTable = "Sellers";

        private readonly IMongoClient mongoClient = new MongoClient();
        private readonly IMongoDatabase mongoDatabase;

        public GetSellersFromMongoToSqlServerCommand(IToysData data)
            : base(data)
        {
            this.mongoDatabase = this.mongoClient.GetDatabase(Arsenicdbinmongodb);
        }

        public override bool Execute()
        {
            var docs = this.LoadSellersFromMongoDb().Result;

            if (this.Data.Sellers.All().Any() || !docs.Any())
            {
                return false;
            }

            this.SaveSellersToSqlServer(docs);

            return true;
        }

        private async Task<List<BsonDocument>> LoadSellersFromMongoDb()
        {
            var collection = this.mongoDatabase.GetCollection<BsonDocument>(SellersTable);
            var dataFromMongo = await collection.Find(x => true).ToListAsync();

            return dataFromMongo;
        }

        private void SaveSellersToSqlServer(IEnumerable<BsonDocument> documents)
        {
            var seller = new Seller();
            
            foreach (var document in documents)
            {
                seller.Name = document["Name"].ToString();

                this.Data.Sellers.Add(seller);
                this.Data.SaveChanges();
            }
        }
    }
}