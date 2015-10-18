namespace Toys.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Threading;
    using Toys.Core.MongoDb;
    using Toys.Data;
    using Toys.XmlReportExporter;

    public class StartUp
    {
        private static void Main()
        {
            var context = new ToysDbContext();
            var db = new ToysData(context);

            Console.WriteLine(db.Products.All().Count());

            var xmlReportsExporter = new XmlReportExporter();
            xmlReportsExporter.ExportReport(context);
        }
    }
}