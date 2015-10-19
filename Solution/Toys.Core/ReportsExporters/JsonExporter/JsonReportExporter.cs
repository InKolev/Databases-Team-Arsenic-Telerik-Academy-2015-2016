namespace Toys.Core.ReportsExporters.JsonExporter
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Toys.Core.ReportsExporters.Contracts;
    using Toys.Core.ReportsExporters.ReportsCommon;

    public class JsonReportExporter : IExporter
    {
        public bool ExportReport(DbContext dbContext)
        {
            var dbDataExtractor = new ReportsDataExtractor();
            var salesList = dbDataExtractor.GetData(dbContext);
            var salesReport = new SalesReport() { Sales = salesList };

            Directory.CreateDirectory(@"..\\..\\..\\Files\\JsonReports");

            foreach(var report in salesReport.Sales)
            {
                var salesReportAsJson = JsonConvert.SerializeObject(report, Formatting.Indented);
                File.WriteAllText(@"..\\..\\..\\Files\\JsonReports\\report" + "-" + report.ID + ".json", salesReportAsJson);
            }

            return true;
        }
    }
}