namespace Toys.Core.XmlExporter
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Toys.Core.ReportsCommon;

    public class XmlReportExporter
    {
        public bool ExportReport(DbContext dbContext)
        {
            var dbDataExtractor = new DbReportsDataExtractor();
            var salesList = dbDataExtractor.GetData(dbContext);
            var salesReport = new SalesReport() { Sales = salesList };

            Directory.CreateDirectory(@"..\\..\\..\\Files\\XmlReports");

            using (var fileStream = File.Create(@"..\\..\\..\\Files\\XmlReports\\report.xml"))
            {
                var xmlSerializer = new XmlSerializer(typeof(SalesReport));
                xmlSerializer.Serialize(fileStream, salesReport);
            }

            return true;
        }
    }
}
