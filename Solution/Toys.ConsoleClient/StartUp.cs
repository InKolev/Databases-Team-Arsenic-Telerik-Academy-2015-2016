namespace Toys.ConsoleClient
{
    using Toys.Data;
    using Toys.Core;

    public class StartUp
    {
        private static void Main()
        {
            var context = new ToysDbContext();
            var db = new ToysData(context);

            var dataManager = new DataManager();
            dataManager.Start();
        }
    }
}