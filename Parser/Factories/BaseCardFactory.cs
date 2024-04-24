using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Factories
{
    abstract class BaseCardFactory<T>
    {
        public abstract Task<T> createCard(string htmlCode);
    }
}
