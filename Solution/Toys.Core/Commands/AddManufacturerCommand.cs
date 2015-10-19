namespace Toys.Core.Commands
{
    using System.Linq;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class AddManufacturerCommand : Command, ICommand
    {
        private const string ZipFilePath = @"../../../Files/DbManufacturersToImportInSqlServer.zip";
        private const string XslFileName = "DbManufacturersToImportInSqlServer.xls";

        public AddManufacturerCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            var dataToImport = this.ImportFromZipFile(ZipFilePath, XslFileName);

            if (this.Data.Manufacturers.All().Any() || !dataToImport.Any())
            {
                return false;
            }

            var manufacturer = new Manufacturer();

            foreach (var item in dataToImport)
            {
                manufacturer.Name = item[1];
                manufacturer.Email = item[2];
                manufacturer.CountryId = int.Parse(item[3]);

                this.Data.Manufacturers.Add(manufacturer);
                this.Data.SaveChanges();
            }

            return true;
        }
    }
}
