using Parser.Models;
using Parser.Parsers.JDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Factories
{
    internal class JDentalAbatmentFactory
    {
        public async Task<JDentalAbatmentCard> createCard(string htmlCode)
        {
            JDentalAbatmentCard card = new JDentalAbatmentCard(htmlCode);
            await card.parseFullData();

            return card;
        }
    }
}
