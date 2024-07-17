using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Models
{
    internal class JDentalAbatmentContainer
    {
        public string Title { get; set; }
        public List<JDentalAbatmentCard> abatmentCards { get; set; } = new List<JDentalAbatmentCard>();
    }
}
