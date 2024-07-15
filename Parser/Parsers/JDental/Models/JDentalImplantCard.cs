using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Models
{
    internal class JDentalImplantCard
    {
        public string Articul { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string ImgUrl { get; set; }

        public bool HavePlugScrew { get; set; } //Винт-заглушка
        public string Neck { get; set; } //Заглушка
        public float HeightNeck { get; set; } //Высота заглушки
        public string Connection { get; set; } //Соединение
        public float Length { get; set; } //Длина
        public float Diametr { get; set; } //Диаметр
        public string Platform { get; set; } //Платформа
        public string Material { get; set; } //Материал
        public float OrthopedSize { get; set; } //Размер ортопедической отвертки


        private string htmlCard;
        private string fullHref;

        public JDentalImplantCard(string htmlCard)
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

            HavePlugScrew = string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Винт-заглушка"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                ) == "есть";

            Neck = string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Шейка"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                );

            float heightNeck = 0.0f;
            int heightNeckIndex = htmlCode.IndexOf("Высота шейки, мм");
            if (heightNeckIndex != -1)
            {
                float.TryParse(string.Concat(
                    htmlCode.Substring(htmlCode.IndexOf("Высота шейки, мм"))
                        .SkipWhile(ch => ch != 'v')
                        .Skip(7)
                        .TakeWhile(ch => ch != '<')
                    ).Replace(".", ","), out heightNeck);
            }

            HeightNeck = heightNeck;

            Connection = string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Соединение"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                );

            float length = 0.0f;
            float.TryParse(string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Длина, мм"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                ).Replace(".", ","), out length);
            Length = length;

            Platform = string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Платформа"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                );

            Material = string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Материал"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                );

            float orthopedSize = 0.0f;
            float.TryParse(string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Размер ортопедической отвертки, мм"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                ).Replace(".", ","), out orthopedSize);
            OrthopedSize = orthopedSize;

            float diametr = 0.0f;
            float.TryParse(string.Concat(
                htmlCode.Substring(htmlCode.IndexOf("Диаметр, мм"))
                    .SkipWhile(ch => ch != 'v')
                    .Skip(7)
                    .TakeWhile(ch => ch != '<')
                ).Replace(".", ","), out diametr);
            Diametr = diametr;

            return true;
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
