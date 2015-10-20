namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Toys.Data.Contracts;

    public class SaveAddressFromXmlToSqlServerCommand : Command, ICommand
    {
        private const string SellersTextFilePath = @"../../../Files/LoadXml/SellersAddress.xml";

        public SaveAddressFromXmlToSqlServerCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            var rootNode = this.GetRootNodeXmlDocument(SellersTextFilePath);
            var address = this.GetAddress(rootNode);

            this.AddAddressToDb(address);

            return true;
        }

        // TODO: Need improvement
        private void AddAddressToDb(List<string> address)
        {
            for (int i = 1; i < address.Count + 1; i++)
            {
                var seller = this.Data.Sellers.GetById(i);
                seller.Address = address[i - 1];

                this.Data.Sellers.Update(seller);
                this.Data.SaveChanges();
            }
        }

        private List<string> GetAddress(XmlNode rootNode)
        {
            var addresses = new List<string>();

            if (rootNode == null)
            {
                throw new ArgumentNullException("rootNode" + "is empty");
            }

            foreach (var address in from XmlNode node in rootNode.ChildNodes
                                    select node["address"] into xmlElement
                                    where xmlElement != null
                                    select xmlElement.InnerText)
            {
                addresses.Add(address);
            }

            return addresses;
        }

        private XmlElement GetRootNodeXmlDocument(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);

            return doc.DocumentElement;
        }
    }
}
