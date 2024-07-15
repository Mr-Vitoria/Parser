using Parser.Factories;
using Parser.Models;
using Parser.Parsers.JDental.Factories;
using Parser.Parsers.JDental.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental
{
    internal class JDentalListParser
    {
        private const string BASE_URL_JDICON = "https://jdentalcare.ru/jdicon";
        private const string BASE_URL_JDICON_PLUS = "https://jdentalcare.ru/jdicon-plus";
        private const string BASE_URL_JDICON_ULTRA_S = "https://jdentalcare.ru/jdicon-ultra-s";
        private const string BASE_URL_JDNASAL = "https://jdentalcare.ru/jdnasal";
        private const string BASE_URL_JDPTERYGO = "https://jdentalcare.ru/jdpterygo";
        private const string BASE_URL_JDEVOLUTION_PLUS = "https://jdentalcare.ru/jdevolution-plus";
        private const string BASE_URL_JDZYGOMA = "https://jdentalcare.ru/jdzygoma";

        public async Task<JDentalImplantContainer> parseImplant(string baseUrl, string title)
        {
            JDentalImplantFactory cardFactory = new JDentalImplantFactory();
            JDentalImplantContainer parsedContainer = new JDentalImplantContainer();
            parsedContainer.Title = title;

            LogWriter.WriteInfo($"Получение страницы товаров для {parsedContainer.Title}", ConsoleColor.Green);

            int page = 1;
            List<string> htmlCards = new List<string>();
            while (true)
            {
                string htmlCode = await getPageContent(baseUrl + "/page/"+page);
                if (htmlCode.Contains("error-404"))
                {
                    break;
                }
                List<string> htmlCardTemplate = new List<string>(htmlCode.Split($"class=\"wrap-prod prod-block"));
                htmlCardTemplate.RemoveAt(0);
                string htmlCard = htmlCardTemplate[htmlCardTemplate.Count - 1];
                htmlCardTemplate[htmlCardTemplate.Count - 1] = htmlCard.Substring(0,htmlCard.IndexOf("<div class=\"pgntn-page-pagination"));

                htmlCards.AddRange(htmlCardTemplate);
                page+=1;
            }
            LogWriter.WriteInfo("Закончено", ConsoleColor.Green);

            for (int i = 1; i < htmlCards.Count(); i++)
            {
                LogWriter.WriteInfo($"Выгрузка {i} товара");

                JDentalImplantCard card = await cardFactory.createCard(htmlCards.ElementAt(i));
                parsedContainer.implantCards.Add(card);

                LogWriter.WriteInfo($"Товар {card.Title} выгружен");
            }

            return parsedContainer;
        }

        public async Task<List<JDentalImplantContainer>> getImplantContainers()
        {
            List<JDentalImplantContainer> containers = new List<JDentalImplantContainer>();
            containers.Add( await parseImplant(BASE_URL_JDICON, "JDIcon"));
            containers.Add( await parseImplant(BASE_URL_JDICON_PLUS, "JDIcon Plus"));
            containers.Add( await parseImplant(BASE_URL_JDICON_ULTRA_S, "JDIcon Ultra S"));
            containers.Add( await parseImplant(BASE_URL_JDNASAL, "JDNasal"));
            containers.Add( await parseImplant(BASE_URL_JDPTERYGO, "JDPterygo"));
            containers.Add( await parseImplant(BASE_URL_JDEVOLUTION_PLUS, "JDEvolution Plus"));
            containers.Add( await parseImplant(BASE_URL_JDZYGOMA, "JDZygoma"));

            return containers;
        }

        public async Task<List<JDentalCollectionCard>> parseCollections()
        {
            JDentalCollectionFactory cardFactory = new JDentalCollectionFactory();
            List<JDentalCollectionCard> parsedCollections = new List<JDentalCollectionCard>();

            LogWriter.WriteInfo($"Получение страницы товаров для инструментов и наборов", ConsoleColor.Green);

            int page = 1;
            List<string> htmlCards = new List<string>();
            while (true)
            {
                string htmlCode = await getPageContent("https://jdentalcare.ru/instrumenty-i-nabory/page/" + page);
                if (htmlCode.Contains("error-404"))
                {
                    break;
                }
                List<string> htmlCardTemplate = new List<string>(htmlCode.Split($"class=\"wrap-prod prod-block"));
                htmlCardTemplate.RemoveAt(0);
                string htmlCard = htmlCardTemplate[htmlCardTemplate.Count - 1];
                htmlCardTemplate[htmlCardTemplate.Count - 1] = htmlCard.Substring(0, htmlCard.IndexOf("<footer"));

                htmlCards.AddRange(htmlCardTemplate);
                page += 1;
            }
            LogWriter.WriteInfo("Закончено", ConsoleColor.Green);

            for (int i = 1; i < htmlCards.Count(); i++)
            {
                LogWriter.WriteInfo($"Выгрузка {i} товара");

                JDentalCollectionCard card = await cardFactory.createCard(htmlCards.ElementAt(i));
                parsedCollections.Add(card);

                LogWriter.WriteInfo($"Товар {card.Title} выгружен");
            }

            return parsedCollections;
        }


        public async Task<string> getPageContent(string pageHref)
        {
            var options = new LaunchOptions
            {
                Headless = true
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(pageHref);

                string content = await page.GetContentAsync();

                return content;

            }
        }
    }
}
