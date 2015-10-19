namespace Toys.Core.JsonExporter
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Toys.Core.Contracts;
    using Toys.Core.ReportsCommon;

    public class JsonReportExporter : IExporter
    {
        public bool ExportReport(DbContext dbContext)
        {
            var dbDataExtractor = new DbReportsDataExtractor();
            var salesList = dbDataExtractor.GetData(dbContext);
            var salesReport = new SalesReport() { Sales = salesList };

            Directory.CreateDirectory(@"..\\..\\..\\Files\\JsonReports");
            //using (File.Create(@"..\\..\\..\\Files\\JsonReports\\report.json")) { }

            var salesReportAsJson = JsonConvert.SerializeObject(salesReport, Formatting.Indented);
            File.WriteAllText(@"..\\..\\..\\Files\\JsonReports\\report.json", salesReportAsJson);

            return true;
        }
    }
}