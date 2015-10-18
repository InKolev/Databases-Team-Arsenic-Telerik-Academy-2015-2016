namespace Toys.Core.Commands
{
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Toys.Data.Contracts;

    public class AddSellersToMongoDbCommand : Command, ICommand
    {
        private const string SellersTextFilePaht = @"../../../Files/DbSellersToImportInMongoDb.txt";
        private const string Arsenicdbinmongodb = "ArsenicDbInMongoDb";
        private const string SellersTable = "Sellers";

        private readonly IMongoClient mongoClient = new MongoClient();
        private readonly IMongoDatabase mongoDatabase;

        public AddSellersToMongoDbCommand(IToysData data)
            : base(data)
        {
            this.mongoDatabase = this.mongoClient.GetDatabase(Arsenicdbinmongodb);
        }

        public override bool Execute()
        {
            var dataToImport = this.ImportFromTextFile(SellersTextFilePaht);
            var records = this.mongoDatabase.GetCollection<BsonDocument>(SellersTable).CountAsync(x => true).Result;

            if (!dataToImport.Any() || records > 0)
            {
                return false;
            }

            foreach (var data in dataToImport)
            {
                this.SaveSellerssToDb(data).Wait();
            }

            return true;
        }

        private async Task SaveSellerssToDb(string[] data)
        {
            var document = new BsonDocument
            {
                { "Id", data[0] },
                { "Name", data[1] }
            };

            var collection = this.mongoDatabase.GetCollection<BsonDocument>(SellersTable);
            await collection.InsertOneAsync(document);
        }
    }
}
