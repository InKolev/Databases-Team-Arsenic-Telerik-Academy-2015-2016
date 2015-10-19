namespace Toys.Core.ReportsCommon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    [Serializable]
    public class Report
    {
        [XmlElement("report-id")]
        public int ID { get; set; }

        [XmlElement("seller-name")]
        public string SellerName { get; set; }

        [XmlAttribute("manufacturer-name")]
        public string ManufacturerName { get; set; }

        [XmlAttribute("email")]
        public string ManufacturerEmail { get; set; }

        [XmlAttribute("country-of-origin")]
        public string CountryOfOrigin { get; set; }

        [XmlElement("product-description")]
        public string ProductDescription { get; set; }

        [XmlElement("quantity")]
        public uint Quantity { get; set; }

        [XmlElement("retail-price")]
        public decimal RetailPrice { get; set; }

        [XmlElement("wholesale-price")]
        public decimal WholesalePrice { get; set; }

        [XmlElement("total-sum")]
        public decimal TotalSum { get; set; }

        [XmlElement("order-date")]
        public DateTime? OrderDate { get; set; }
    }
}