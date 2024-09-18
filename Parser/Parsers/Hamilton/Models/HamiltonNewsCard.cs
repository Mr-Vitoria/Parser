using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace Parser.Parsers.Hamilton.Models
{
    internal class HamiltonNewsCard
    {
        private const string URL_MEDIA = "https://hamiltonapps.ru/wp-json/wp/v2/media/";
        public int id { get; set; }
        public string date { get; set; }
        public string date_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string modified { get; set; }
        public int post_views_count { get; set; }
        public RenderedModel title { get; set; }
        public RenderedModel content { get; set; }
        public RenderedModel excerpt { get; set; }
        public int featured_media { get; set; }
        public int[] categories { get; set; }
        public List<int> types { get; set; } = new List<int>();

        public ImageModel? image { get; set; }

        public async Task<bool> getFullData()
        {
            image = await getImageById(featured_media);

            for (int i = 0; i < categories.Length; i++)
            {
                types.Add(getTypeIdFromCategoryId(categories[i]));
            }

            return true;
        }

        private int getTypeIdFromCategoryId(int categoryId)
        {
            switch (categoryId)
            {
                case 3959:
                    return 14;
                case 3960:
                    return 13;
                case 3961:
                    return 12;
                case 3957:
                    return 15;
                case 3962:
                    return 11;
                default:
                    return -1;
            }
        }

        public async Task<ImageModel> getImageById(int imageId)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(URL_MEDIA + imageId);
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ImageModel>(jsonString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
