using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.Hamilton.Models
{
    internal class HamiltonCaseCard
    {
        public int id { get; set; }
        public string date { get; set; }
        public string date_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string modified { get; set; }
        public RenderedModel title { get; set; }
        public RenderedModel content { get; set; }
    }
}
