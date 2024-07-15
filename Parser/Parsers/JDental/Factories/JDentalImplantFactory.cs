using Parser.Models;
using Parser.Parsers.JDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Factories
{
    internal class JDentalImplantFactory
    {
        public async Task<JDentalImplantCard> createCard(string htmlCode)
        {
            JDentalImplantCard card = new JDentalImplantCard(htmlCode);
            await card.parseFullData();

            return card;
        }
    }
}
