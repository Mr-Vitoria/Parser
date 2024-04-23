//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

//using System.Net;


using Parser;

HTMLParser parser = new HTMLParser("https://www.gentlemens.kz/", 
    "js-product"
    );
await parser.parse();

//using ClosedXML.Excel;

//var workbook = new XLWorkbook();
//var worksheet = workbook.Worksheets.Add("Sample Sheet");
//worksheet.Cell("A1").Value = "Hello World!";
//workbook.SaveAs("HelloWorld.xlsx");
