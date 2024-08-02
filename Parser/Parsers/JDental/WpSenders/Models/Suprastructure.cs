using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.WpSenders.Models
{
    internal class Suprastructure
    {
        public string articul { get; set; }
        public string title { get; set; }
        public float price { get; set; }
        public int image { get; set; }

        public string[] lineImplant { get; set; }
        public float diametr { get; set; }
        public string connection { get; set; }
        public float heightGum { get; set; }
        public string platform { get; set; }
        public string material { get; set; }
        public float orthopedSize { get; set; }
        public string impressionLevel { get; set; }
        public string applicationMethod { get; set; }
        public string indications { get; set; }

    }
}
