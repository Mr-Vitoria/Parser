using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Models
{
    internal class JDentalBaseCard
    {
        public string Articul { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string ImgUrl { get; set; }

        public string LineImplant { get; set; } //Линейка имплантатов
        public string Connection { get; set; } //Соединение
        public float Diametr { get; set; } //Диаметр
        public float HeightGum { get; set; } //Высота десны
        public string Platform { get; set; } //Платформа
        public string Material { get; set; } //Материал
        public float OrthopedSize { get; set; } //Размер ортопедической отвертки
        public string ImpressionLevel { get; set; } //Уровень снятия оттиска
        public string ApplicationMethod { get; set; } //Метод применения
        public string Indications { get; set; } //Показания


        private string htmlCard;
        private string fullHref;

        public JDentalBaseCard(string htmlCard)
        {
            this.htmlCard = htmlCard;

            parseBaseData();
        }

        public void parseBaseData()
        {
            ImgUrl = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("<img class=\"product-image\" src=\""))
                    .Skip(32)
                    .TakeWhile(ch => ch != '\"')
                );

            Title = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("h3"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != '<')
                );

            Articul = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("artikul"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != '<')
                );

            string priceString = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("\"price"))
                    .SkipWhile(ch => ch != '>')
                    .Skip(1)
                    .TakeWhile(ch => ch != "₽"[0])
                );
            priceString = priceString.Replace(" ", "");
            Price = int.Parse(priceString);

            fullHref = string.Concat(
                htmlCard.Substring(htmlCard.IndexOf("href=\""))
                    .SkipWhile(ch => ch != '\"')
                    .Skip(1)
                    .TakeWhile(ch => ch != '\"')
                );

            return;
        }

        public async Task<bool> parseFullData()
        {
            string htmlCode = await getPageContent(fullHref);

            htmlCode = htmlCode.Substring(htmlCode.IndexOf("list-options"));
            htmlCode = htmlCode.Substring(0, htmlCode.IndexOf("</div>"));

            LineImplant = getStringValue(htmlCode, "Линейка имплантатов");

            Connection = getStringValue(htmlCode, "Соединение");

            Diametr = getFloatValue(htmlCode, "Диаметр, мм");

            HeightGum = getFloatValue(htmlCode, "Высота десны, мм");
            if(HeightGum == -1.0f)
            {
                HeightGum = getFloatValue(htmlCode, "Высота, мм");
            }
            
            Platform = getStringValue(htmlCode, "Платформа");

            Material = getStringValue(htmlCode, "Материал");

            OrthopedSize = getFloatValue(htmlCode, "Размер ортопедической отвертки, мм");

            ImpressionLevel = getStringValue(htmlCode, "Уровень снятия оттиска");

            ApplicationMethod = getStringValue(htmlCode, "Метод применения");

            Indications = getStringValue(htmlCode, "Показания");

            return true;
        }

        private float getFloatValue(string htmlCode, string fieldTitle)
        {
            int valueIndex = htmlCode.IndexOf(fieldTitle);
            float value = -1.0f;

            if (valueIndex != -1)
            {
                float.TryParse(string.Concat(
                    htmlCode.Substring(valueIndex)
                        .SkipWhile(ch => ch != 'v')
                        .Skip(7)
                        .TakeWhile(ch => ch != '<')
                    ).Replace(".", ","), out value);
            }

            return value;
        }

        private string getStringValue(string htmlCode, string fieldTitle)
        {
            int valueIndex = htmlCode.IndexOf(fieldTitle);
            if(valueIndex != -1)
            {
                return string.Concat(
                    htmlCode.Substring(valueIndex)
                        .SkipWhile(ch => ch != 'v')
                        .Skip(7)
                        .TakeWhile(ch => ch != '<')
                    );
            }
            return "";
        }

        public async Task<string> getPageContent(string pageHref)
        {
            var options = new LaunchOptions
            {
                Headless = true
            };

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(pageHref);

                string content = await page.GetContentAsync();

                return content;

            }
        }
    }
}
