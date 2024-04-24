//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

using Parser;
using Parser.Factories;
using Parser.Models;
using Parser.Parsers;

GentlemenListHTMLParser parser = new GentlemenListHTMLParser(
    "https://www.gentlemens.kz/", 
    "js-product"
    );
List<GentlemenCard> gentlemenCards = await parser.parse();

new ExcelWriter().writeGentlemenCard(gentlemenCards);
