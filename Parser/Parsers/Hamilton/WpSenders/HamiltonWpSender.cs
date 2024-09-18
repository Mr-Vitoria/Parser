using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Parsers.Hamilton.Models;
using Parser.Parsers.Hamilton.WpSenders.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace Parser.Parsers.Hamilton.WpSenders
{
    internal class HamiltonWpSender
    {
        private const string BASE_URL = "https://hamilton.paagoo.ru/admin/api/wp/v2/";

        private const string USERNAME = "sibserv";
        private const string PASSWORD = "pUFf r9Zv nyNr DnAM 97v7 W2em";


        public async Task<bool> sendNews(List<HamiltonNewsCard> cards)
        {
            try
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    HamiltonNewsCard card = cards[i];
                    var options = new RestClientOptions(BASE_URL)
                    {
                        Authenticator = new HttpBasicAuthenticator(USERNAME, PASSWORD)
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("news");
                    request.AddJsonBody(new WpObject<News>
                    {
                        title = card.title.rendered,
                        status = "publish",
                        date = card.date,
                        modified = card.modified,
                        fields = new News
                        {
                            content = card.content.rendered,
                            description = card.excerpt.rendered,
                            poster = await sendImageFromUrl(card.image.source_url, $"{card.image.id}"),
                            type = card.types.ToArray(),
                            view_count = card.post_views_count
                        }
                    });
                    await client.PostAsync(request);
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteInfo(ex.Message);
            }

            return true;
        }

        public async Task<bool> sendCases(List<HamiltonCaseCard> cards)
        {
            try
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    HamiltonCaseCard card = cards[i];
                    var options = new RestClientOptions(BASE_URL)
                    {
                        Authenticator = new HttpBasicAuthenticator(USERNAME, PASSWORD)
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("case");
                    request.AddJsonBody(new WpObject<Case>
                    {
                        title = card.title.rendered,
                        status = "publish",
                        date = card.date,
                        modified = card.modified,
                        fields = new Case
                        {
                            content = card.content.rendered,
                            author_description = "",
                            author_post = "",
                            description = "",
                            short_title = ""
                                                    
                        }
                    });
                    await client.PostAsync(request);
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteInfo(ex.Message);
            }

            return true;
        }


        public async Task<int> sendImageFromUrl(string imageUrl, string title)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    if (!Directory.Exists("temp"))
                        Directory.CreateDirectory("temp");

                    client.DownloadFile(new Uri(imageUrl), @$"temp\{title}.jpg");
                }

                string url = BASE_URL + "media";
                string credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(USERNAME + ":" + PASSWORD));

                var headers = new Dictionary<string, string>
                                        {
                                            { "Authorization", $"Basic {credentials}" },
                                            { "Accept", "application/json" },
                                        };

                using (HttpClient client = new())
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    StreamContent streamContent = new(new FileStream(@$"temp\{title}.jpg", FileMode.Open));
                    streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    streamContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; filename={title}.jpg");

                    using var response = await client.PostAsync(url, streamContent);
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic obj = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                        return obj.id ?? -1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteInfo(ex.Message);
            }
            return -1;
        }

    }
}
