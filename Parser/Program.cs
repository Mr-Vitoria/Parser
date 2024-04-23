//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

using Parser;
using Parser.Factories;
using Parser.Models;
using Parser.Parsers;

GentlemenHTMLParser parser = new GentlemenHTMLParser(
    "https://www.gentlemens.kz/", 
    "js-product"
    );
List<GentlemenCard> gentlemenCards = await parser.parse(new GentlemenCardFactory());

new ExcelWriter().writeGentlemenCard(gentlemenCards);
