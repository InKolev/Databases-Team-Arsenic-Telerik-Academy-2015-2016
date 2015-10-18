namespace Toys.ConsoleClient
{
    using Toys.Core;

    public class StartUp
    {
        private static void Main()
        {
            var dataManager = new DataManager();
            dataManager.Start();
        }
    }
}