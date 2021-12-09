using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PageEvents : PdfPageEventHelper
    {
        private PdfContentByte _contentByte;
        private List<PdfTemplate> _templates;

        public PageEvents()
        {
            this._templates = new List<PdfTemplate>();
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            Camping campingModel = new Camping();
            var results = campingModel.SelectLast();

            this._contentByte = writer.DirectContentUnder;
            PdfTemplate templateBottom = this._contentByte.CreateTemplate(50, 50);
            PdfTemplate templateTop = this._contentByte.CreateTemplate(50, 50);
            this._templates.Add(templateBottom);
            this._templates.Add(templateTop);

            int pageN = writer.CurrentPageNumber;
            string pageFooter = "\nPagina " + pageN.ToString() + " van de ";
            string pageHeader = "Reserveringsoverzicht: " + results.Name;
            Image image = Image.GetInstance(".\\Images\\caravan-solid.png");
            image.SetAbsolutePosition(36, 800);
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            this._contentByte.BeginText();
            this._contentByte.SetFontAndSize(baseFont, 10);
            this._contentByte.SetTextMatrix(document.RightMargin, document.PageSize.GetBottom(document.BottomMargin));
            this._contentByte.ShowText(pageFooter + "" + (writer.PageNumber));
            this._contentByte.EndText();
            this._contentByte.AddTemplate(templateBottom, document.RightMargin, 100);

            this._contentByte.BeginText();
            this._contentByte.AddImage(image);
            this._contentByte.SetFontAndSize(baseFont, 20);
            this._contentByte.SetTextMatrix(document.LeftMargin, document.PageSize.GetTop(document.TopMargin));
            this._contentByte.ShowText(pageHeader);
            this._contentByte.EndText();
            this._contentByte.AddTemplate(templateTop, document.LeftMargin, document.PageSize.GetTop(document.TopMargin));
        }
    }
}
