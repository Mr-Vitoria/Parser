using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Models
{
    internal class JDentalBaseContainer
    {
        public string Title { get; set; }
        public List<JDentalBaseCard> cards { get; set; } = new List<JDentalBaseCard>();
    }
}
