using ClosedXML.Excel;
using Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class ExcelWriter
    {
        public bool writeGentlemenCard(List<GentlemenCard> gentlemenCards)
        {
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Карточки товара");

            //Headers
            worksheet.Cell("A1").Value = "Название";
            worksheet.Cell("B1").Value = "Описание";
            worksheet.Cell("C1").Value = "Url изображения";

            for (int i = 0; i < gentlemenCards.Count; i++)
            {
                worksheet.Cell($"A{i + 2}").Value = gentlemenCards[i].Title;
                worksheet.Cell($"B{i + 2}").Value = gentlemenCards[i].Description;
                worksheet.Cell($"C{i + 2}").Value = gentlemenCards[i].ImgUrl;
            }


            workbook.SaveAs("GentlemenCards.xlsx");

            return true;
        }
    }
}
