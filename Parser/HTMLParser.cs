using Parser.Factories;
using Parser.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class HTMLParser<T>
    {
        private readonly string baseUrl;
        private string classNameCard;

        public HTMLParser(string baseUrl, string classNameCard)
        {
            this.baseUrl = baseUrl;
            this.classNameCard = classNameCard;
        }

        public async Task<List<T>> parse(BaseCardFactory<T> cardFactory)
        {
            List<T> parsedCards = new List<T>();

            string htmlCode = await getHtmlCode(baseUrl);

            IEnumerable<string> htmlCards = htmlCode.Split($"class=\"{classNameCard} ");
            for (int i = 1; i < htmlCards.Count(); i++)
            {
                parsedCards.Add(cardFactory.createCard(htmlCards.ElementAt(i)));
            }

            return parsedCards;
        }

        private async Task<string> getHtmlCode(string url)
        {
            var options = new LaunchOptions
            {
                Headless = true
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(url);

                //TODO - дождаться полной загрузки страницы, вместо 5 секунд
                Thread.Sleep(5000);

                string content = await page.GetContentAsync();
                return content;
            }
        }

    }
}
