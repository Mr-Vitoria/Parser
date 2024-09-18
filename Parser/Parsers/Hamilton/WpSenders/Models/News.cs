using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.Hamilton.WpSenders.Models
{
    internal class News
    {
        public string description { get; set; }
        public int[] type { get; set; }
        public int poster { get; set; }
        public string content { get; set; }
        public int view_count { get; set; }
    }
}
