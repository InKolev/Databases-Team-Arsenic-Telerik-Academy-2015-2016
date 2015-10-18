namespace Toys.Core.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Driver;
    using Toys.Data;
    using Toys.Data.Contracts;
    using Toys.Models;
    using JsonConvert = Newtonsoft.Json.JsonConvert;


    public class MongoDbOperations
    {
        private const string CsvFilePath = @"../../../Files/DbProductsToImportInMongoDb.txt";
        private IMongoClient client;
        private IMongoDatabase database;
        private IToysData sqlServerDb;


        public MongoDbOperations(IToysData context)
        {
            this.client = new MongoClient();
            this.database = this.client.GetDatabase("ArsenicDb");
            this.sqlServerDb = context;
        }

        public void ImportProducts()
        {
            var path = CsvFilePath;

            using (StreamReader reader = new StreamReader(path))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    var data = line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    this.SaveProductsToDb(data).Wait();

                    line = reader.ReadLine();

                    Console.Write('.');
                }
            }
        }

        public async Task<List<BsonDocument>> LoadProductsFromMongoDb()
        {
            var collection = this.database.GetCollection<BsonDocument>("Products");

            var dbDocuments = await collection.Find(x => true).ToListAsync();

            //foreach (var document in dbDocuments)
            //{
            //    this.SaveProductsToSqlServer(document);
            //}

            return dbDocuments;


            //this.SaveProductsToSqlServer(dbDocuments);
        }

        public void SaveProductsToSqlServer(List<BsonDocument> documents)
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

                this.sqlServerDb.Products.Add(product);


                try
                {
                    this.sqlServerDb.SaveChanges();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }
                
            }
            //var product = new Product
            //{

            //};

            //this.sqlServerDb.Products.Add(product);
            //this.sqlServerDb.SaveChanges();
        }

        private async Task SaveProductsToDb(string[] data)
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
