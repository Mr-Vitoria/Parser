using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.WpSenders.Models
{
    internal class Implant
    {
        public string articul { get; set; }
        public string lineImplant { get; set; }
        public string title { get; set; }
        public float price { get; set; }
        public string plugScrew { get; set; }
        public string neck { get; set; }
        public float heightNeck { get; set; }
        public string connection { get; set; }
        public float length { get; set; }
        public float diametr { get; set; }
        public string platform { get; set; }
        public string material { get; set; }
        public float orthopedSize { get; set; }
        public int image { get; set; }
    }
}
