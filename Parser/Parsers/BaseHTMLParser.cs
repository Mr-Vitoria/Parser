using Parser.Factories;
using Parser.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers
{
    abstract class BaseHTMLParser<T>
    {
        public int LOAD_PAGE_TIMEOUT = 1000;
        public int COUNT_TRY_LOAD_CONTENT = 10;

        protected string baseUrl;
        protected string classNameCard;

        public abstract Task<List<T>> parse();

        public virtual async Task<string> getPageContent()
        {
            var options = new LaunchOptions
            {
                Headless = true
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(baseUrl);

                return await page.GetContentAsync();
            }
        } 
    }
}
