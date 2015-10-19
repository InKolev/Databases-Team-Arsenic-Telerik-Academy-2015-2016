using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toys.Core.Contracts
{
    public interface IExporter
    {
        bool ExportReport(DbContext dbContext);
    }
}
