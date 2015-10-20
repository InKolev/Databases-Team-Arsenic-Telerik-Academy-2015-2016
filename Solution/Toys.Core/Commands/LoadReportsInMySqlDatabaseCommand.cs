namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MySql.Data;
    using MySql.Data.MySqlClient;
    using Toys.Core.ReportsExporters.ReportsCommon;
    using System.Data.Entity;
    using Toys.Data;

    public class LoadReportsInMySqlDatabaseCommand : Command
    {
        private const string ConnectionString = "server=127.0.0.1;user=root;port=3306;password=c17h25n0;";
        private const string DatabaseName = "Reports";
        private const string TableName = "SalesReports";
        private const string CreateDatabaseQuery =
            "DROP DATABASE IF EXISTS " + DatabaseName + "; " +
            "CREATE DATABASE " + DatabaseName;
        private const string CreateTableQuery =
            "DROP TABLE IF EXISTS " + TableName + "; " +
                "CREATE TABLE " + TableName +
                "(" +
                    "ID int PRIMARY KEY, " +
                    "SellerName varchar(150) CHARSET utf8, " +
                    "ManufacturerName varchar(150) CHARSET utf8, " +
                    "ManufacturerEmail varchar(150) CHARSET utf8, " +
                    "CountryOfOrigin varchar(150) CHARSET utf8, " +
                    "ProductDescription varchar(150) CHARSET utf8, " +
                    "Quantity int, " +
                    "RetailPrice decimal, " +
                    "WholesalePrice decimal, " +
                    "TotalSum decimal, " +
                    "OrderDate datetime " +
                ")ENGINE=InnoDB;";

        public LoadReportsInMySqlDatabaseCommand(ToysData data, ToysDbContext dbContext)
            : base(data)
        {
            this.DbContext = dbContext;
        }

        public ToysDbContext DbContext { get; set; }

        public MySqlConnection Connection { get; set; }

        public MySqlCommand Command { get; set; }

        public SalesReport Report { get; set; }

        public void ConnectToMySQLDatabase()
        {
            this.Connection = new MySqlConnection(ConnectionString);
            this.Connection.Open();

            if (this.Connection.State.ToString().Equals("Open"))
            {
                Console.WriteLine("Connecting to MySQL Server successfull.");
            }
            else
            {
                Console.WriteLine("Connection to MySQL Server failed.");
            }
        }

        public void CreateDatabaseSchema()
        {
            this.Command = new MySqlCommand(CreateDatabaseQuery, this.Connection);
            this.Command.ExecuteNonQuery();

            this.Connection.ChangeDatabase(DatabaseName);

            this.Command = new MySqlCommand(CreateTableQuery, this.Connection);
            this.Command.ExecuteNonQuery();

            if (this.Connection.Database.ToString().Equals(DatabaseName))
            {
                Console.WriteLine("Database creation successfull.");
            }
            else
            {
                Console.WriteLine("Database creation failed.");
            }
        }

        public void LoadReports()
        {
            foreach (var report in this.Report.Sales)
            {
                var insertQuery = this.CreateInsertQuery(report);
                this.Command = new MySqlCommand(insertQuery, this.Connection);
                this.Command.ExecuteNonQuery();
            }
        }

        public void CreateSalesReport()
        {
            var dataExtractor = new ReportsDataExtractor();
            var reportsList = dataExtractor.GetData(this.DbContext);
            this.Report = new SalesReport() { Sales = reportsList };
        }

        public string CreateInsertQuery(Report report)
        {
            var query =
              "INSERT INTO " + TableName +
              "(ID, SellerName, ManufacturerName, ManufacturerEmail, CountryOfOrigin, ProductDescription, Quantity, RetailPrice, WholesalePrice, TotalSum) " +
              "VALUES(" +
              (int)report.ID + ", '" +
              report.SellerName + "', '" +
              report.ManufacturerName + "', '" +
              report.ManufacturerEmail + "', '" +
              report.CountryOfOrigin + "', '" +
              report.ProductDescription + "', " +
              (int)report.Quantity + ", " +
              report.RetailPrice + ", " +
              report.WholesalePrice + ", " +
              report.TotalSum + ");";

            return query;
        }

        public override bool Execute()
        {
            this.ConnectToMySQLDatabase();
            this.CreateDatabaseSchema();
            this.CreateSalesReport();
            this.LoadReports();

            return true;
        }
    }
}