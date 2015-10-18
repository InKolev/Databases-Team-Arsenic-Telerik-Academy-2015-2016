namespace Toys.Core.Commands
{
    using System.Linq;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class AddCountryCommand : Command, ICommand
    {
        private const string CsvFilePath = @"../../../Files/DbCountriesToImportInSqlServer.txt";

        public AddCountryCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            var dataToImport = this.ImportFromTextFile(CsvFilePath);

            if (this.Data.Countries.All().Any() || !dataToImport.Any())
            {
                return false;
            }

            var country = new Country();

            foreach (var item in dataToImport)
            {
                country.Name = item[1];
                country.CapitalCity = item[2];
                country.PhonePrefix = item[3];
                country.IsoCode = item[4];
                country.Continent = item[5];

                this.Data.Countries.Add(country);
                this.Data.SaveChanges();
            }

            return true;
        }
    }
}
