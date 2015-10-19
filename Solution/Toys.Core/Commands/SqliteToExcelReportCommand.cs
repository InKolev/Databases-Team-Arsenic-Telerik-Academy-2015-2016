namespace Toys.Core.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Toys.Data.Contracts;
    using Toys.Models;
    using Excel = Microsoft.Office.Interop.Excel;

    public class SqliteToExcelReportCommand : Command, ICommand
    {
        private const string ExcelPath = "SqliteData.xlsx";
        public SqliteToExcelReportCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            var products = this.Data.Products.All().ToList();

            this.CreateProductsExcelReport(products);

            return true;
        }

        private void CreateProductsExcelReport(List<Product> products)
        {
            if (File.Exists(ExcelPath))
            {
                File.Delete(ExcelPath);
            }

            Excel.Application oApp = new Excel.Application();

            Excel.Worksheet oSheet;
            Excel.Workbook oBook;

            oBook = oApp.Workbooks.Add();
            oSheet = (Excel.Worksheet)oBook.Worksheets.get_Item(1);

            long counter = 2;
            oSheet.Cells[1, 1] = "Id";
            oSheet.Cells[1, 2] = "Sku";
            oSheet.Cells[1, 3] = "Description";
            oSheet.Cells[1, 4] = "WholesalePrice";
            oSheet.Cells[1, 5] = "RetailPrice";
            oSheet.Cells[1, 6] = "TradeDiscount";
            oSheet.Cells[1, 7] = "TradeDiscountRate";

            foreach (var product in products)
            {
                oSheet.Cells[counter, 1] = product.Id;
                oSheet.Cells[counter, 2] = product.Sku;
                oSheet.Cells[counter, 3] = product.Description;
                oSheet.Cells[counter, 4] = product.WholesalePrice;
                oSheet.Cells[counter, 5] = product.RetailPrice;
                oSheet.Cells[counter, 6] = product.TradeDiscount;
                oSheet.Cells[counter, 7] = product.TradeDiscountRate;
                counter++;
            }

            oBook.SaveAs(ExcelPath);
            oBook.Close();
            oApp.Quit();

        }
    }
}