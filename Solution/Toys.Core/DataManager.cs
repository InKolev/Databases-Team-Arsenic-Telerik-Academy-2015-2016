namespace Toys.Core
{
    using System;
    using System.Collections.Generic;
    using Toys.Core.Commands;
    using Toys.Data;

    public class DataManager
    {
        private readonly ToysDbContext context = new ToysDbContext();
        private readonly ToysData data;

        public DataManager()
        {
            this.data = new ToysData(this.context);
        }

        public void Start()
        {
            ICollection<ICommand> commands = new List<ICommand>
            {
                new AddCountryCommand(this.data),
                new AddManufacturerCommand(this.data),
                new AddProductsToMongoDbCommand(this.data),
                new AddSellersToMongoDbCommand(this.data),
                new GetProductsFromMongoToSqlServerCommand(this.data),
                new GetSellersFromMongoToSqlServerCommand(this.data),
                new SalesReportGeneratorCommand(this.data),
                new ZipSalesReportsCommand(this.data),
                new AddSalesReportsToSqlCommand(this.data),
                new PdfCommand(this.data),
                new ExportXmlReportCommand(this.data, this.context),
                new ExportJsonReportCommand(this.data, this.context),
                new SqliteToExcelReportCommand(this.data),
                new LoadReportsInMySqlDatabaseCommand(this.data, this.context)
            };

            foreach (var command in commands)
            {
                if (command.Execute())
                {
                    Console.WriteLine("Command sucessful - " + command.GetType().Name);
                }
                else
                {
                    Console.WriteLine("Data already exist - " + command.GetType().Name);
                }
            }
        }
    }
}