using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Models
{
    internal class JDentalCollectionCard
    {
        public string Articul { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public List<string> Properties { get; set; } = new List<string>();

        private string htmlCard;
        private string fullHref;

        public JDentalCollectionCard(string htmlCard)
        {
            this.htmlCard = htmlCard;

            parseBaseData();
        }

        public void parseBaseData()
        {
            ImgUrl = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("<img class=\"product-image\" src=\""))
                    .Skip(32)
                    .TakeWhile(ch => ch != '\"')
                );

            Title = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("h3"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != '<')
                );

            Articul = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("artikul"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != '<')
                );

            fullHref = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("href=\""))
                    .SkipWhile(ch => ch != '\"')
                    .Skip(1)
                    .TakeWhile(ch => ch != '\"')
                );

            return;
        }

        public async Task<bool> parseFullData()
        {
            string htmlCode = await getPageContent(fullHref);

            htmlCode = htmlCode.Substring(htmlCode.IndexOf("kit-list"));
            htmlCode = htmlCode.Substring(0, htmlCode.IndexOf("</div>"));

            IEnumerable<string> propertyHtmls = htmlCode.Split("<li>");

            for (int i = 1; i < propertyHtmls.Count(); i++)
            {
                string element = propertyHtmls.ElementAt(i);
                Properties.Add(string.Concat(
                element.TakeWhile(ch => ch != '<')
                ));
            }

            return true;
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
