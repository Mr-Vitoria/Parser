using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Models
{
    internal class JDentalImplantContainer
    {
        public string Title { get; set; }
        public List<JDentalImplantCard> implantCards { get; set; } = new List<JDentalImplantCard>();
    }
}
