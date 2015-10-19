namespace Toys.Core.Commands
{
    using Toys.Data.Contracts;

    class ZipSalesReportsCommand : Command, ICommand
    {
        public ZipSalesReportsCommand(IToysData data)
            : base(data)
        {           
        }

        public override bool Execute()
        {
            var salesReportsPath = @"../../../Files/SalesReports";
           
            if (this.ZipFile(salesReportsPath))
            {
                return true;
            }           

            return false;
        }
    }
}
