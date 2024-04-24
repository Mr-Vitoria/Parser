using Parser.Factories;
using Parser.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers
{
    internal class GentlemenListHTMLParser : BaseHTMLParser<GentlemenCard>
    {

        public GentlemenListHTMLParser(string baseUrl, string classNameCard)
        {
            base.baseUrl = baseUrl;
            base.classNameCard = classNameCard;
        }

        public override async Task<List<GentlemenCard>> parse()
        {
            GentlemenCardFactory cardFactory = new GentlemenCardFactory();
            List<GentlemenCard> parsedCards = new List<GentlemenCard>();

            LogWriter.WriteInfo("Получение страницы товаров", ConsoleColor.Green);

            string htmlCode = await getPageContent();
            LogWriter.WriteInfo("Закончено", ConsoleColor.Green);

            IEnumerable<string> htmlCards = htmlCode.Split($"class=\"{classNameCard} ");
            for (int i = 1; i < htmlCards.Count(); i++)
            {
                LogWriter.WriteInfo($"Выгрузка {i} товара");

                GentlemenCard card = await cardFactory.createCard(htmlCards.ElementAt(i));
                parsedCards.Add(card);
                
                LogWriter.WriteInfo($"Товар {card.Title} выгружен");
            }

            return parsedCards;
        }


        public override async Task<string> getPageContent()
        {
            var options = new LaunchOptions
            {
                Headless = true
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(baseUrl);

                try
                {
                    await FirstLoadPage(page);
                    await LoadAllCardList(page);
                }
                catch (Exception){}


                string content = await page.GetContentAsync();

                return content;

            }

            async Task LoadAllCardList(IPage page)
            {
                int countLoad = 0;
                do
                {
                    await page.TapAsync(".js-store-load-more-btn");
                    Thread.Sleep(LOAD_PAGE_TIMEOUT);

                    countLoad++;

                } while (countLoad < COUNT_TRY_LOAD_CONTENT);
            }

            async Task FirstLoadPage(IPage page)
            {
                int countLoad = 0;
                do
                {
                    var handle = (await page.EvaluateExpressionHandleAsync($"document.querySelector('.js-store-load-more-btn')"));
                    if (handle.RemoteObject.Description != null)
                    {
                        break;
                    }
                    Thread.Sleep(LOAD_PAGE_TIMEOUT);

                    countLoad++;

                } while (countLoad < COUNT_TRY_LOAD_CONTENT);
            }
        }
    }
}
