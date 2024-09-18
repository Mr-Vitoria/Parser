using Newtonsoft.Json;
using Parser.Parsers.Hamilton.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.Hamilton
{
    internal class HamiltonParser
    {
        public string BASE_URL = "https://hamiltonapps.ru/wp-json/wp/v2";

        public async Task<List<HamiltonNewsCard>> getNewsCards()
        {
            LogWriter.WriteInfo($"Получение страницы новостей", ConsoleColor.Green);

            int page = 1;
            List<HamiltonNewsCard> newsCard = new List<HamiltonNewsCard>();
            while (true)
            {
                string jsonString = await getJsonString(BASE_URL + "/posts?per_page=100&page=" + page);
                if (jsonString == "" || jsonString.Contains("rest_post_invalid_page_number"))
                {
                    break;
                }

                List<HamiltonNewsCard>? cards = JsonConvert.DeserializeObject<List<HamiltonNewsCard>>(jsonString);

                for (int i = 0; i < cards.Count; i++)
                {
                    LogWriter.WriteInfo($"Получение информации для " + cards[i].title.rendered, ConsoleColor.Green);
                    await cards[i].getFullData();
                    LogWriter.WriteInfo($"Закончено", ConsoleColor.Green);
                }

                newsCard.AddRange(cards);
                page += 1;
            }
            LogWriter.WriteInfo("Закончено", ConsoleColor.Green);

            return newsCard;
        }

        public async Task<List<HamiltonCaseCard>> getCaseCards()
        {
            LogWriter.WriteInfo($"Получение страниц историй успехов", ConsoleColor.Green);

            List<HamiltonCaseCard> caseCards = new List<HamiltonCaseCard>();
            string jsonString = await getJsonString(BASE_URL + "/pages?parent=52768");
            if (jsonString == "" || jsonString.Contains("rest_post_invalid_page_number"))
            {
                return caseCards;
            }

            caseCards = JsonConvert.DeserializeObject<List<HamiltonCaseCard>>(jsonString) ?? new List<HamiltonCaseCard>();

            LogWriter.WriteInfo("Закончено", ConsoleColor.Green);

            return caseCards;
        }

        public async Task<string> getJsonString(string pageHref)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(pageHref);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
