namespace Toys.Core.Commands
{
    using Bytescout.Spreadsheet;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Toys.Data;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class SqliteToExcelReportCommand : Command, ICommand
    {
        private const string ExcelPath = @"../../../Files/SqliteData.xlsx";
        public SqliteToExcelReportCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            // uncomment this for seed the sqlite database
            //var products = this.Data.Products.All().ToList();

            //this.AddDataToSqlite(products);

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

            Spreadsheet document = new Spreadsheet();
            Worksheet sheet = document.Workbook.Worksheets.Add("Product Reports");

            int counter = 2;
            sheet.Cell(1, 1).Value = "Id";
            sheet.Cell(1, 2).Value = "Sku";
            sheet.Cell(1, 3).Value = "Description";
            sheet.Cell(1, 4).Value = "WholesalePrice";
            sheet.Cell(1, 5).Value = "RetailPrice";
            sheet.Cell(1, 6).Value = "TradeDiscount";
            sheet.Cell(1, 7).Value = "TradeDiscountRate";

            foreach (var product in products)
            {
                sheet.Cell(counter, 1).Value = product.Id;
                sheet.Cell(counter, 2).Value = product.Sku;
                sheet.Cell(counter, 3).Value = product.Description;
                sheet.Cell(counter, 4).Value = product.WholesalePrice;
                sheet.Cell(counter, 5).Value = product.RetailPrice;
                sheet.Cell(counter, 6).Value = product.TradeDiscount;
                sheet.Cell(counter, 7).Value = product.TradeDiscountRate;
                counter++;
            }

            document.SaveAs(ExcelPath);
        }
    }
}