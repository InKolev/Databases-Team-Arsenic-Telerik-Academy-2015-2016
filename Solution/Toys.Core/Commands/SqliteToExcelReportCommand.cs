namespace Toys.Core.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Toys.Data;
    using Toys.Data.Contracts;
    using Toys.Models;
    using GemBox.Spreadsheet;

    public class SqliteToExcelReportCommand : Command, ICommand
    {
        private const string ExcelPath = @"../../../Files/LoadXml/SqliteData.xlsx";
        public SqliteToExcelReportCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            // uncomment this for seed the sqlite database
            var products = this.Data.Products.All().ToList();

            this.AddDataToSqlite(products);

            this.CreateProductsExcelReport();

            return true;
        }

        private void AddDataToSqlite(List<Product> products)
        {
            using (var db = new ToysSqliteContext())
            {
                var sqliteProducts = db.Products;

                var sqliteProductsCount = db.Products.Count();
                if (sqliteProductsCount != products.Count)
                {
                    foreach (var item in products)
                    {
                        var product = new Product()
                        {
                            Sku = item.Sku,
                            Description = item.Description,
                            WholesalePrice = item.WholesalePrice,
                            RetailPrice = item.RetailPrice,
                            TradeDiscount = item.TradeDiscount,
                            TradeDiscountRate = item.TradeDiscountRate,
                            ManufacturerId = item.ManufacturerId
                        };

                        sqliteProducts.Add(product);
                    }
                }
                db.SaveChanges();
            }
        }

        private void CreateProductsExcelReport()
        {
            List<Product> products;
            using (var db = new ToysSqliteContext())
            {
                products = db.Products.Where(i => i.Id == i.Id).ToList();
            }

            if (File.Exists(ExcelPath))
            {
                File.Delete(ExcelPath);
            }

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            ExcelFile ef = new ExcelFile();
            ExcelWorksheet ws = ef.Worksheets.Add("Hello World");

            int counter = 2;
            ws.Cells[1, 1].Value = "Id";
            ws.Cells[1, 2].Value = "Sku";
            ws.Cells[1, 3].Value = "Description";
            ws.Cells[1, 4].Value = "WholesalePrice";
            ws.Cells[1, 5].Value = "RetailPrice";
            ws.Cells[1, 6].Value = "TradeDiscount";
            ws.Cells[1, 7].Value = "TradeDiscountRate";

            foreach (var product in products)
            {
                ws.Cells[counter, 1].Value = product.Id;
                ws.Cells[counter, 2].Value = product.Sku;
                ws.Cells[counter, 3].Value = product.Description;
                ws.Cells[counter, 4].Value = product.WholesalePrice;
                ws.Cells[counter, 5].Value = product.RetailPrice;
                ws.Cells[counter, 6].Value = product.TradeDiscount;
                ws.Cells[counter, 7].Value = product.TradeDiscountRate;
                counter++;
            }

            ef.Save("test.xls");
        }
    }
}