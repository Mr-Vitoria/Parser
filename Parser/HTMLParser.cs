using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class HTMLParser
    {
        private readonly string baseUrl;
        private string classNameModel;
        
        public HTMLParser(string baseUrl, string classNameModel)
        {
            this.baseUrl = baseUrl;
            this.classNameModel = classNameModel;
        }

        public async Task<List<GentlemenCard>> parse()
        {
            List<GentlemenCard> parsedData = new List<GentlemenCard>();

            string htmlCode = await getHtmlCode(baseUrl);

                StringBuilder parsedCode = new StringBuilder(htmlCode);

                List<string> classes = htmlCode.Split($"class=\"{classNameModel} ").ToList();
            for (int i = 1; i < classes.Count; i++)
            {
                GentlemenCard card = new GentlemenCard(classes[i]);
                card.parse();

                parsedData.Add(
                    card    
                    );
            }

            return parsedData;
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

                // Wait for the page to finish loading and the JavaScript code to execute
                Thread.Sleep(5000);

                // Scrape the page content
                string content = await page.GetContentAsync();
                return content;
            }
        }

    }
}
