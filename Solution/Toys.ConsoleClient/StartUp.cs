namespace Toys.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Threading;
    using Toys.Data;
    using Toys.Core;
    using Toys.Core.JsonExporter;

    public class StartUp
    {
        private static void Main()
        {
            var context = new ToysDbContext();
            var db = new ToysData(context);

            //var jsonReportExporter = new JsonReportExporter();
            //jsonReportExporter.ExportReport(context);

            var dataManager = new DataManager();
            dataManager.Start();
        }
    }
}