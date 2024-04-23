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

        public GentlemenCard(string htmlCard)
        {
            this.htmlCard = htmlCard;
        }

        public bool parse()
        {
            ImgUrl = string.Concat(htmlCard.Substring(
                htmlCard.IndexOf("data-original=\"") + 15
                ).TakeWhile(ch => ch != '\"'));

            Title = string.Concat(htmlCard.Substring(
                htmlCard.IndexOf("js-store-prod-name")
                ).SkipWhile(ch => ch != '>').Skip(1).TakeWhile(ch => ch != '<'));


            Description = "";

            return true;
        }
    }
}
