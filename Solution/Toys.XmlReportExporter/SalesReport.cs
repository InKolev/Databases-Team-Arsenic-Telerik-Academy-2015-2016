namespace Toys.XmlReportExporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("sales-reports")]
    public class SalesReport
    {
        [XmlElement("sale")]
        public List<Report> Sales { get; set; }
    }
}