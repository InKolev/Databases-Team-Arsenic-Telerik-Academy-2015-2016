using System;
using System.Collections.Generic;
using System.Linq;
namespace Toys.Core.Commands
{
    using System.Text;
    using System.Threading.Tasks;
    using Toys.Data.Contracts;
    using Toys.Models;


    class AddSalesReportsToSqlCommand : Command, ICommand
    {
        private const string ZipFilePath = @"../../../Files/SalesReports/SalesReports.zip";
        private const string ExtractDir = @"../../../Files/SalesReportsUnzipped/";

        public AddSalesReportsToSqlCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            var dataToImport = this.ImportReportsFromZipFile(ZipFilePath, ExtractDir);

            if (this.Data.Sales.All().Any() || !dataToImport.Any())
            {
                return false;
            }

            foreach (var item in dataToImport)
            {
                var sale = new Sale();
                sale.ProductId = int.Parse(item[0]);
                sale.Sku = item[1];
                sale.Quantity = int.Parse(item[2]);
                sale.SellerId = int.Parse(item[3]);
                sale.Date = DateTime.Parse(item[4]);

                this.Data.Sales.Add(sale);
                this.Data.SaveChanges();
            }

            return true;
        }
    }
}
