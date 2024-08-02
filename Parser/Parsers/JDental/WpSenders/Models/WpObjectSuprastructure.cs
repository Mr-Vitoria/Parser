using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.WpSenders.Models
{
    internal class WpObjectSuprastructure<T>:WpObject<T>
    {
        public string[] type_suprastructure { get; set; }
    }
}
