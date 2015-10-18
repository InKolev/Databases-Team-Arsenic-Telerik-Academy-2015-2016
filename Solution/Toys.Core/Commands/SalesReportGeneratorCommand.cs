namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.IO;
    using System.Linq;
    using Toys.Data.Contracts;

    public class SalesReportGeneratorCommand : Command, ICommand
    {
        private const string SaveDirFilePath = @"../../../Files/SalesReports/";
        private static readonly Random Rand = new Random();

        public SalesReportGeneratorCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            if (Directory.EnumerateFileSystemEntries(SaveDirFilePath).Any())
            {
                return false;
            }

            return this.Generate();
        }

        private bool Generate()
        {
            var list = new List<string[]>();
            var count = 0;
            var totalProducts = this.Data.Products.All().Count();
            var totalSellers = this.Data.Sellers.All().Count();

            if (totalProducts <= 0 || totalSellers <= 0)
            {
                return false;
            }

            for (int i = 0; i < 500; i++)
            {
                var record = new string[4];

                var productId = this.Data.Products.GetById(Rand.Next(1, totalProducts)).Id;
                var sku = this.Data.Products.GetById(productId).Sku;
                var quantity = Rand.Next(1, totalSellers);
                var sellerId = Rand.Next(1, totalSellers);

                record[0] = productId.ToString();
                record[1] = sku;
                record[2] = quantity.ToString();
                record[3] = sellerId.ToString();

                list.Add(record);
                count++;

                if (count != 20)
                {
                    continue;
                }

                // this.SaveRecordsToTextFile(list);
                this.SaveRecordsToExcelFile(list);
                count = 0;
                list.Clear();
            }

            return true;
        }

        private void SaveRecordsToTextFile(List<string[]> records)
        {
            var fileName = this.GetRandomDate(new DateTime(2000, 1, 1), DateTime.Now).ToString("dd MMM yyyy").Replace(' ', '-');

            using (var writer = new StreamWriter(SaveDirFilePath + fileName + ".txt", true))
            {
                foreach (var record in records)
                {
                    writer.WriteLine(string.Join(";", record));
                }
            }
        }

        private void SaveRecordsToExcelFile(List<string[]> records)
        {
            var fileName = this.GetRandomDate(new DateTime(2000, 1, 1), DateTime.Now).ToString("dd MMM yyyy").Replace(' ', '-');
            var provider = @"Provider=Microsoft.Jet.OLEDB.4.0;";
            var dataSource = @"Data Source=" + SaveDirFilePath + fileName + ".xls;";
            var extendedProperties = @"Extended Properties=""Excel 8.0;HDR=NO;""";

            string connectionString = provider + dataSource + extendedProperties;

            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (DbCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = "CREATE TABLE [Sheet1] (F1 number, F2 char(255), F3 number, F4 number)";
                    command.ExecuteNonQuery();

                    foreach (var record in records)
                    {
                        var dataToInsert = string.Format("INSERT INTO [Sheet1$] (F1, F2, F3, F4) VALUES({0},\"{1}\",{2},{3})", record[0], record[1], record[2], record[3]);

                        command.CommandText = dataToInsert;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(Rand.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }
    }
}