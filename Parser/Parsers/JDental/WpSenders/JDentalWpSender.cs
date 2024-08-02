using Newtonsoft.Json;
using Parser.Parsers.JDental.Models;
using Parser.Parsers.JDental.WpSenders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net;

namespace Parser.Parsers.JDental.WpSenders
{
    internal class JDentalWpSender
    {
        private const string WP_URL = "https://jdental.ru/wp-json/wp/v2";

        private const string USERNAME = "jd_adm";
        private const string PASSWORD = "t0QG tcm1 l7OC rUba ta10 v0Rk";

        public async Task<bool> sendImplants(JDentalImplantContainer container)
        {
            try
            {
                for (int i = 0; i < container.implantCards.Count; i++)
                {
                    JDentalImplantCard card = container.implantCards[i];
                    var options = new RestClientOptions(WP_URL)
                    {
                        Authenticator = new HttpBasicAuthenticator(USERNAME, PASSWORD)
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("implant");
                    request.AddJsonBody(new WpObject<Implant>
                    {
                        title = card.Title,
                        status = "publish",
                        fields = new Implant
                        {
                            articul = card.Articul,
                            lineImplant = getImplantLineLabel(container.Title),
                            title = card.Title,
                            price = card.Price,
                            neck = card.Neck,
                            heightNeck = card.HeightNeck,
                            connection = card.Connection,
                            length = card.Length,
                            diametr = card.Diametr,
                            platform = card.Platform,
                            material = card.Material,
                            orthopedSize = card.OrthopedSize,
                            image = await sendImageFromUrl(card.ImgUrl, "image_"+container.Title+"_"+i)
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

        public async Task<bool> sendCollection(JDentalCollectionCard collection, int index)
        {
            try
            {
                var options = new RestClientOptions(WP_URL)
                {
                    Authenticator = new HttpBasicAuthenticator(USERNAME, PASSWORD)
                };
                var client = new RestClient(options);
                var request = new RestRequest("collections");
                StringBuilder propertyString = new StringBuilder();
                for (int i = 0; i < collection.Properties.Count; i++)
                {
                    propertyString.Append(collection.Properties[i] + @"\r\n");
                }
                request.AddJsonBody(new WpObject<Collection>
                {
                    title = collection.Title,
                    status = "publish",
                    fields = new Collection
                    {
                        articul = collection.Articul,
                        title = collection.Title,
                        properties = propertyString.ToString(),
                        image = await sendImageFromUrl(collection.ImgUrl, "image_collection_"+index)
                    }
                });
                await client.PostAsync(request);
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

                string url = WP_URL + "/media";
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

        private string getImplantLineLabel(string implantLine)
        {
            switch (implantLine)
            {
                case "JDEvolution Plus":
                    return "8";
                case "JDIcon":
                    return "3";
                case "JDIcon Plus":
                    return "4";
                case "JDIcon Ultra S":
                    return "5";
                case "JDNasal":
                    return "6";
                case "JDPterygo":
                    return "7";
                case "JDZygoma":
                    return "9";
                default:
                    return "-1";
            }
        }
    }
}
