namespace Toys.Core.Commands
{
    using System;
    using System.Linq;
    using Toys.Core.ReportsExporters.XmlExporter;
    using Toys.Data;
    using Toys.Data.Contracts;

    public class ExportXmlReportCommand : Command
    {
        public ExportXmlReportCommand(IToysData data, ToysDbContext dbContext)
            : base(data)
        {
            this.DbContext = dbContext;
        }

        private ToysDbContext DbContext { get; set; }

        public override bool Execute()
        {
            var xmlReportsExporter = new XmlReportExporter();
            var exportSuccessfull = xmlReportsExporter.ExportReport(this.DbContext);

            return exportSuccessfull;
        }
    }
}