using System;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace Arcmatics.PdfManager
{
    public partial class Footer : PdfPageEventHelper
    {
        Int16 pageNumber = 0;

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Paragraph footer = new Paragraph("Talentsprofile.com", FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL));
            footer.Alignment = Element.ALIGN_CENTER;

            PdfPTable footerTbl = new PdfPTable(new float[] { 500, 120 });
            footerTbl.TotalWidth = 500;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell1 = new PdfPCell(footer);
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell1.Border = 1;
            cell1.PaddingLeft = 10;
            footerTbl.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(footer);
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            // Increament page number:
            pageNumber++;
            cell2.Phrase = new Phrase(pageNumber.ToString(), FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL));
            cell2.Border = 1;

            footerTbl.AddCell(cell2);
            footerTbl.WriteSelectedRows(0, -1, 50, document.Bottom, writer.DirectContent);
        }
    }
}
