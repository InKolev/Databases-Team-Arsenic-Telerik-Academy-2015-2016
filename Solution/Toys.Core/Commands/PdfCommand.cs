namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using Toys.Data.Contracts;
    using Toys.Models;

    public class PdfCommand : Command, ICommand
    {
        private const string SaveDirFilePath = @"../../../Files/Pdf/";
        private const string FontPath = @"../../../Files/Font/arialuni.ttf";
        private const int BaseFontSize = 10;

        public PdfCommand(IToysData data)
            : base(data)
        {
        }

        public override bool Execute()
        {
            var sellers = this.Data.Sellers.All().ToList();
            var products = this.Data.Products.All().Where(x => x.WholesalePrice > 200).ToList();

            if (!sellers.Any())
            {
                return false;
            }

            this.CreateSellersPdfReport(sellers);
            this.CreateProductsPdfReport(products);
            return true;
        }

        private void CreateProductsPdfReport(List<Product> products)
        {
            var output = new FileStream(SaveDirFilePath + "Products" + "_" + DateTime.Now.ToString("d").Replace("/", "-") + ".pdf", FileMode.Create);
            var document = new Document();

            var pdfWriter = PdfWriter.GetInstance(document, output);
            pdfWriter.CloseStream = false;
            document.Open();

            var fontPath = FontPath;
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var smallFont = new Font(baseFont, BaseFontSize - 1);
            var normalFont = new Font(baseFont, BaseFontSize);
            var boldFont = new Font(baseFont, BaseFontSize, Font.BOLD);

            FontFactory.RegisterDirectory(SaveDirFilePath, true);
            FontFactory.Register(fontPath, "Arial Unicode MS");
            FontFactory.RegisterFamily("Arial Unicode MS", "Arial Unicode MS", fontPath);

            this.AddProductsToPdf(document, products, normalFont, smallFont, boldFont);

            document.Close();
        }

        private void CreateSellersPdfReport(List<Seller> sellers)
        {
            var output = new FileStream(SaveDirFilePath + "Sellers" + "_" + DateTime.Now.ToString("d").Replace("/", "-") + ".pdf", FileMode.Create);
            var document = new Document();

            var pdfWriter = PdfWriter.GetInstance(document, output);
            pdfWriter.CloseStream = false;
            document.Open();

            var fontPath = FontPath;
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            var smallFont = new Font(baseFont, BaseFontSize - 1);
            var normalFont = new Font(baseFont, BaseFontSize);
            var boldFont = new Font(baseFont, BaseFontSize, Font.BOLD);

            FontFactory.RegisterDirectory(SaveDirFilePath, true);
            FontFactory.Register(fontPath, "Arial Unicode MS");
            FontFactory.RegisterFamily("Arial Unicode MS", "Arial Unicode MS", fontPath);

            this.AddSellersToPdf(document, sellers, normalFont, smallFont, boldFont);

            document.Close();
        }

        private void AddSellersToPdf(Document document, List<Seller> sellers, Font normalFont, Font smallFont, Font boldFont)
        {
            var title = ("List of Sellers " + DateTime.Now).ToUpper();

            document.AddTitle(title);

            var table = new PdfPTable(2)
            {
                WidthPercentage = 100,
            };

            var tableTitle = new PdfPCell(this.Paragraph(title, boldFont, "CENTER"))
            {
                Colspan = 2,
                Padding = 10,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP
            };

            table.AddCell(tableTitle);

            table.AddCell(new PdfPCell(this.Paragraph("ID", boldFont)) { Padding = 10, VerticalAlignment = Element.ALIGN_TOP, BackgroundColor = BaseColor.ORANGE });
            table.AddCell(new PdfPCell(this.Paragraph("ИМЕ", boldFont)) { Padding = 10, VerticalAlignment = Element.ALIGN_TOP, BackgroundColor = BaseColor.ORANGE });

            var count = 0;
            foreach (var seller in sellers)
            {
                var cellId = new PdfPCell() { Padding = 10, VerticalAlignment = Element.ALIGN_TOP };
                var cellName = new PdfPCell() { Padding = 10, VerticalAlignment = Element.ALIGN_TOP };

                if (count % 2 != 0)
                {
                    cellId.BackgroundColor = BaseColor.GRAY;
                    cellName.BackgroundColor = BaseColor.GRAY;
                }

                cellId.AddElement(this.Paragraph(seller.Id.ToString(), normalFont));
                cellName.AddElement(this.Paragraph(seller.Name, normalFont));

                table.AddCell(cellId);
                table.AddCell(cellName);

                count++;
            }

            document.Add(table);
        }

        private void AddProductsToPdf(Document document, List<Product> products, Font normalFont, Font smallFont, Font boldFont)
        {
            var title = ("List of Products " + DateTime.Now).ToUpper();

            document.AddTitle(title);

            var table = new PdfPTable(4)
            {
                WidthPercentage = 100,
            };

            var tableTitle = new PdfPCell(this.Paragraph(title, boldFont, "CENTER"))
            {
                Colspan = 4,
                Padding = 10,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP
            };
            table.AddCell(tableTitle);

            table.AddCell(new PdfPCell(this.Paragraph("ID", boldFont)) { Padding = 10, VerticalAlignment = Element.ALIGN_TOP, BackgroundColor = BaseColor.ORANGE });
            table.AddCell(new PdfPCell(this.Paragraph("SKU", boldFont)) { Padding = 10, VerticalAlignment = Element.ALIGN_TOP, BackgroundColor = BaseColor.ORANGE });
            table.AddCell(new PdfPCell(this.Paragraph("ИМЕ", boldFont)) { Padding = 10, VerticalAlignment = Element.ALIGN_TOP, BackgroundColor = BaseColor.ORANGE });
            table.AddCell(new PdfPCell(this.Paragraph("ТЪРГОВСКА ЦЕНА", boldFont)) { Padding = 10, VerticalAlignment = Element.ALIGN_TOP, BackgroundColor = BaseColor.ORANGE });

            var count = 0;
            decimal totalPrice = 0.0m;

            foreach (var product in products)
            {
                var id = new PdfPCell() { Padding = 10, VerticalAlignment = Element.ALIGN_TOP };
                var sku = new PdfPCell() { Padding = 10, VerticalAlignment = Element.ALIGN_TOP };
                var desc = new PdfPCell() { Padding = 10, VerticalAlignment = Element.ALIGN_TOP };
                var price = new PdfPCell() { Padding = 10, VerticalAlignment = Element.ALIGN_TOP };

                if (count % 2 != 0)
                {
                    id.BackgroundColor = BaseColor.GRAY;
                    sku.BackgroundColor = BaseColor.GRAY;
                    desc.BackgroundColor = BaseColor.GRAY;
                    price.BackgroundColor = BaseColor.GRAY;
                }

                id.AddElement(this.Paragraph(product.Id.ToString(), normalFont));
                sku.AddElement(this.Paragraph(product.Sku, normalFont));
                desc.AddElement(this.Paragraph(product.Description, normalFont));
                price.AddElement(this.Paragraph(product.WholesalePrice.ToString(CultureInfo.InvariantCulture) + " лв.", normalFont));

                table.AddCell(id);
                table.AddCell(sku);
                table.AddCell(desc);
                table.AddCell(price);

                count++;
                totalPrice += product.WholesalePrice;
            }

            var tableFootLeft = new PdfPCell(this.Paragraph("ОБЩА ЦЕНА", boldFont, "RIGHT"))
            {
                Colspan = 3,
                Padding = 10,
                BackgroundColor = BaseColor.PINK,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP
            };

            var tableFootRight = new PdfPCell(this.Paragraph(totalPrice.ToString(CultureInfo.InvariantCulture) + " лв.", boldFont, "LEFT"))
            {
                Colspan = 1,
                Padding = 10,
                BackgroundColor = BaseColor.PINK,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP
            };

            table.AddCell(tableFootLeft);
            table.AddCell(tableFootRight);

            document.Add(table);
        }

        private Paragraph Paragraph(string text, Font font, string alignment = "JUSTIFIED")
        {
            var paragraph = new Paragraph(text, font);

            switch (alignment)
            {
                case "JUSTIFIED":
                    paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                    break;
                case "LEFT":
                    paragraph.Alignment = Element.ALIGN_LEFT;
                    break;
                case "RIGHT":
                    paragraph.Alignment = Element.ALIGN_RIGHT;
                    break;
                case "CENTER":
                    paragraph.Alignment = Element.ALIGN_CENTER;
                    break;
            }

            return paragraph;
        }
    }
}
