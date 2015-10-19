using System;
using System.Collections.Generic;
using System.Linq;
namespace Toys.Core.Commands
{
    using System.Text;
    using System.Threading.Tasks;
    using Toys.Data.Contracts;


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

            //if (this.Data.Countries.All().Any() || !dataToImport.Any())
            //{
            //    return false;
            //}

            //var country = new Country();
            //
            //foreach (var item in dataToImport)
            //{
            //    country.Name = item[1];
            //    country.CapitalCity = item[2];
            //    country.PhonePrefix = item[3];
            //    country.IsoCode = item[4];
            //    country.Continent = item[5];
            //
            //    this.Data.Countries.Add(country);
            //    this.Data.SaveChanges();
            //}

            return true;
        }
    }
}
