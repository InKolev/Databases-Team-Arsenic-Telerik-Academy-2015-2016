namespace Toys.Core.Commands
{
    using System;
    using System.Linq;
    using Toys.Data;
    using Toys.Data.Contracts;
    using Toys.XmlReportExporter;

    public class ExportXmlReportCommand : Command
    {
        public ExportXmlReportCommand(IToysData data, ToysDbContext dbContext)
            : base(data)
        {
            this.DbContext = dbContext;
        }

          public override bool Execute()
          {
              var xmlReportsExporter = new XmlReportExporter();
              var exportSuccessfull = xmlReportsExporter.ExportReport(this.DbContext);
        
              return exportSuccessfull;
          }
        
          private ToysDbContext DbContext { get; set; }
    }
}