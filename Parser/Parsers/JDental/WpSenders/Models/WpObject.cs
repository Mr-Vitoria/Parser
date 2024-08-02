using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.WpSenders.Models
{
    internal class WpObject<T>
    {
        public string title { get; set; }
        public string status { get; set; }
        public T fields { get; set; }
    }
}
