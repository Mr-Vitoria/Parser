using Parser.Models;
using Parser.Parsers.JDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Factories
{
    internal class JDentalBaseFactory
    {
        public async Task<JDentalBaseCard> createCard(string htmlCode)
        {
            JDentalBaseCard card = new JDentalBaseCard(htmlCode);
            await card.parseFullData();

            return card;
        }
    }
}
