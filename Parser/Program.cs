//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

//using System.Net;

//using ClosedXML.Excel;

using Parser;
using Parser.Factories;
using Parser.Models;

HTMLParser<GentlemenCard> parser = new HTMLParser<GentlemenCard>(
    "https://www.gentlemens.kz/", 
    "js-product"
    );
List<GentlemenCard> gentlemenCards = await parser.parse(new GentlemenCardFactory());

new ExcelWriter().writeGentlemenCard(gentlemenCards);
