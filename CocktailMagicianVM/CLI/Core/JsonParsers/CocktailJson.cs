using CLI.Core.JasonParsers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.JasonParsers
{
    public class CocktailJson : ICocktailJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Ingredient { get; set; }
    }
}
