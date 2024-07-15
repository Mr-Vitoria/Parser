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
        public async Task<JDentalImplantContainer> parseJDIconImplant()
        {
            JDentalImplantFactory cardFactory = new JDentalImplantFactory();
            JDentalImplantContainer parsedContainer = new JDentalImplantContainer();
            parsedContainer.Title = "JDIcon";

            LogWriter.WriteInfo($"Получение страницы товаров для {parsedContainer.Title}", ConsoleColor.Green);

            int page = 1;
            List<string> htmlCards = new List<string>();
            while (true)
            {
                string htmlCode = await getPageContent("https://jdentalcare.ru/jdicon/page/"+page);
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
