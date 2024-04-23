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
    internal class GentlemenHTMLParser : BaseHTMLParser<GentlemenCard>
    {
        public GentlemenHTMLParser(string baseUrl, string classNameCard)
        {
            base.baseUrl = baseUrl;
            base.classNameCard = classNameCard;
        }

        public override async Task<List<GentlemenCard>> parse(BaseCardFactory<GentlemenCard> cardFactory)
        {
            List<GentlemenCard> parsedCards = new List<GentlemenCard>();

            string htmlCode = await HTMLFunctions.getHtmlCode(baseUrl);

            IEnumerable<string> htmlCards = htmlCode.Split($"class=\"{classNameCard} ");
            for (int i = 1; i < htmlCards.Count(); i++)
            {
                parsedCards.Add(await cardFactory.createCard(htmlCards.ElementAt(i)));
            }

            return parsedCards;
        }

    }
}
