namespace Toys.Core.Commands
{
    using System;
    using Toys.Data.Contracts;

    public class GetSalesReportsFromZipToSqlServerCommand : Command, ICommand
    {
        public GetSalesReportsFromZipToSqlServerCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}