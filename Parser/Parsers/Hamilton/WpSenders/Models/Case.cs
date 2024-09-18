using DocumentFormat.OpenXml.Office.CoverPageProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.Hamilton.WpSenders.Models
{
    internal class Case
    {
        public string short_title { get; set; }
        public string description { get; set; }
        public string author_description { get; set; }
        public string author_post { get; set; }
        public string content { get; set; }
    }
}
