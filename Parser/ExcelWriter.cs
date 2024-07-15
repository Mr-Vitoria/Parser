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
            worksheet.Cell("C1").Value = "Url изображения";
            worksheet.Cell("B1").Value = "Описание";

            for (int i = 0; i < gentlemenCards.Count; i++)
            {
                worksheet.Cell($"A{i + 2}").Value = gentlemenCards[i].Title;
                worksheet.Cell($"C{i + 2}").Value = gentlemenCards[i].ImgUrl;
                worksheet.Cell($"B{i + 2}").Value = gentlemenCards[i].Description;
            }


            workbook.SaveAs("data/GentlemenCards.xlsx");

            return true;
        }
    }
}
