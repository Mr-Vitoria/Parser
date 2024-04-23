using Parser.Factories;
using Parser.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Parsers
{
    abstract class BaseHTMLParser<T>
    {
        protected string baseUrl;
        protected string classNameCard;

        public abstract Task<List<T>> parse(BaseCardFactory<T> cardFactory);

    }
}
