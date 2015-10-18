namespace Toys.XmlReportExporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public class XmlReportExporter
    {
        public bool ExportReport(object dbContext)
        {
            var dbDataExtractor = new DbReportsDataExtractor();

            var salesList = dbDataExtractor.GetData(dbContext);
            var salesReport = new SalesReport() { Sales = salesList };

            Directory.CreateDirectory(@"..\\..\\..\\XmlReports");

            using (var fileStream = File.Create(@"..\\..\\..\\XmlReports\\report.xml"))
            {
                var xmlSerializer = new XmlSerializer(typeof(SalesReport));
                xmlSerializer.Serialize(fileStream, salesReport);
            }

            return true;
        }
    }
}
