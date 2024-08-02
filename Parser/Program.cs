//Выгрузить с сайта https://www.gentlemens.kz/
//Название + ссылка на картинку + описание товара
//Записать полученные данные в таблицу Excel

using Parser;
using Parser.Models;
using Parser.Parsers;
using Parser.Parsers.JDental;
using Parser.Parsers.JDental.Models;
using Parser.Parsers.JDental.WpSenders;

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


#region JDental

JDentalListParser parser = new JDentalListParser();

LogWriter.WriteInfo("Парсинг страницы начат", ConsoleColor.Red);

//List<JDentalImplantContainer> implantContainers = await parser.getImplants();
List<JDentalCollectionCard> collections = await parser.getCollections();
LogWriter.WriteInfo("Парсинг страницы завершен", ConsoleColor.Green);

LogWriter.WriteInfo("Отправка данных на Wordpress", ConsoleColor.Red);
JDentalWpSender sender = new JDentalWpSender();
//for (int i = 0; i < implantContainers.Count; i++)
//{
//    LogWriter.WriteInfo($"Контейнер {implantContainers[i].Title}", ConsoleColor.DarkMagenta);

//    await sender.sendImplants(implantContainers[i]);
//}

for (int i = 0; i < collections.Count; i++)
{
    LogWriter.WriteInfo($"Коллекция {collections[i].Title}", ConsoleColor.DarkMagenta);

    await sender.sendCollection(collections[i], i);
}

LogWriter.WriteInfo("Отправка данных на Wordpress закончена", ConsoleColor.Green);

//List<JDentalBaseContainer> suprastructures = await parser.getSuprastructures();
//await parser.parseCollections();
#endregion