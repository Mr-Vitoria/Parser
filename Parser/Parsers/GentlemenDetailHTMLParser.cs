﻿using Parser.Factories;
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
    internal class GentlemenDetailHTMLParser : BaseHTMLParser<GentlemenCard>
    {

        public GentlemenDetailHTMLParser(string baseUrl, string classNameCard)
        {
            base.baseUrl = baseUrl;
            base.classNameCard = classNameCard;
        }

        public override async Task<List<GentlemenCard>> parse()
        {
            return null;
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
                }
                catch (Exception){}

                string content = await page.GetContentAsync();

                return content;

            }

            async Task FirstLoadPage(IPage page)
            {
                int countLoad = 0;
                do
                {
                    var handle = (await page.EvaluateExpressionHandleAsync($"document.querySelector('.js-store-prod-all-text')"));
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
