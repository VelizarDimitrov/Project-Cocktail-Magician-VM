using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.JasonParsers.Contracts
{
    public interface ICocktailJson
    {
        string Name { get; set; }
        string Description { get; set; }
        string[] Ingredient { get; set; }
    }
}
