using Parser.Models;
using Parser.Parsers.JDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers.JDental.Factories
{
    internal class JDentalCollectionFactory
    {
        public async Task<JDentalCollectionCard> createCard(string htmlCode)
        {
            JDentalCollectionCard card = new JDentalCollectionCard(htmlCode);
            await card.parseFullData();

            return card;
        }
    }
}
