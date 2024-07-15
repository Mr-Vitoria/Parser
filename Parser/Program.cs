//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

using Parser;
using Parser.Models;
using Parser.Parsers;
using Parser.Parsers.JDental;
using Parser.Parsers.JDental.Models;

//GentlemenListHTMLParser parser = new GentlemenListHTMLParser(
//    "https://www.gentlemens.kz/", 
//    "js-product"
//    );


//LogWriter.WriteInfo("Парсинг страницы начат", ConsoleColor.Red);

//List<GentlemenCard> gentlemenCards = await parser.parse();

//LogWriter.WriteInfo("Парсинг страницы завершен", ConsoleColor.Green);


//LogWriter.WriteInfo("Выгрузка полученных данных в таблицу Excel", ConsoleColor.Red);

//new ExcelWriter().writeGentlemenCard(gentlemenCards);

//LogWriter.WriteInfo("Завершено", ConsoleColor.Green);

JDentalListParser parser = new JDentalListParser();

LogWriter.WriteInfo("Парсинг страницы начат", ConsoleColor.Red);

JDentalImplantContainer implantContainer = await parser.parseJDIconImplant();

LogWriter.WriteInfo("Парсинг страницы завершен", ConsoleColor.Green);
