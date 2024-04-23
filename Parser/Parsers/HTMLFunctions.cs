using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers
{
    internal class HTMLFunctions
    {
        public static async Task<string> getHtmlCode(string url)
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
