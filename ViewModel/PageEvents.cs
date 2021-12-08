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
        //constructor
        public PageEvents()
        {
            this._templates = new List<PdfTemplate>();
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            Camping campingModel = new Camping();
            var results = campingModel.SelectLast();

            _contentByte = writer.DirectContentUnder;
            PdfTemplate templateBottom = _contentByte.CreateTemplate(50, 50);
            PdfTemplate templateTop = _contentByte.CreateTemplate(50, 50);
            _templates.Add(templateBottom);
            _templates.Add(templateTop);

            int pageN = writer.CurrentPageNumber;
            string pageFooter = "\nPagina " + pageN.ToString() + " van de ";
            string pageHeader = "Reserveringsoverzicht: " + results.Name;
            Image image = Image.GetInstance(".\\Images\\caravan-solid.png");
            image.SetAbsolutePosition(36, 800);
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            _contentByte.BeginText();
            _contentByte.SetFontAndSize(baseFont, 10);
            _contentByte.SetTextMatrix(document.RightMargin, document.PageSize.GetBottom(document.BottomMargin));
            _contentByte.ShowText(pageFooter + "" + (writer.PageNumber));
            _contentByte.EndText();
            _contentByte.AddTemplate(templateBottom, document.RightMargin, 100);

            _contentByte.BeginText();
            _contentByte.AddImage(image);
            _contentByte.SetFontAndSize(baseFont, 20);
            _contentByte.SetTextMatrix(document.LeftMargin, document.PageSize.GetTop(document.TopMargin));
            _contentByte.ShowText(pageHeader);
            _contentByte.EndText();
            _contentByte.AddTemplate(templateTop, document.LeftMargin, document.PageSize.GetTop(document.TopMargin));
        }
    }
}
