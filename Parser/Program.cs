//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

//using System.Net;

//using ClosedXML.Excel;

using Parser;

HTMLParser parser = new HTMLParser("https://www.gentlemens.kz/", 
    "js-product"
    );
List<GentlemenCard> gentlemenCards = await parser.parse();

new ExcelWriter().writeGentlemenCard(gentlemenCards);
