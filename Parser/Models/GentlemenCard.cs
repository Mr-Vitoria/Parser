using Parser.Parsers;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Models
{
    internal class GentlemenCard
    {
        private string htmlCard;

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public string HrefToFullCard { get; set; }

        public GentlemenCard(string htmlCard)
        {
            this.htmlCard = htmlCard;

            parseBaseData();
        }

        public void parseBaseData()
        {
            ImgUrl = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("data-original=\""))
                    .Skip(15)
                    .TakeWhile(ch => ch != '\"')
                );

            Title = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("js-store-prod-name"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != '<')
                );


            Description = "";

            HrefToFullCard = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("href=\""))
                    .Skip(6)
                    .TakeWhile(ch => ch != '\"')
                );
        }

        public async Task<bool> parseFullData()
        {
            string htmlCode = await new GentlemenDetailHTMLParser(HrefToFullCard,"").getPageContent();

            Description = string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("js-store-prod-all-text"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != '<')
                );

            return true;
        }
    }
}
