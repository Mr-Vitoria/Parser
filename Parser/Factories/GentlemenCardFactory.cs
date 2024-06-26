﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Models;

namespace Parser.Factories
{
    internal class GentlemenCardFactory : BaseCardFactory<GentlemenCard>
    {
        public override async Task<GentlemenCard> createCard(string htmlCode)
        {
            GentlemenCard card = new GentlemenCard(htmlCode);
            await card.parseFullData();

            return card;
        }
    }
}
