namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Toys.Core.ReportsExporters.JsonExporter;
    using Toys.Data;
    using Toys.Data.Contracts;

    public class ExportJsonReportCommand : Command
    {
        public ExportJsonReportCommand(IToysData data, ToysDbContext dbContext)
            : base(data)
        {
            this.DbContext = dbContext;
        }

        private ToysDbContext DbContext { get; set; }

        public override bool Execute()
        {
            var jsonReportExporter = new JsonReportExporter();
            var exportSuccessfull = jsonReportExporter.ExportReport(this.DbContext);

            return exportSuccessfull;
        }
    }
}
