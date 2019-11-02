using System;
using System.Collections.Generic;
using System.Text;

namespace Data.SolutionPreLoad.JsonParsers.Contracts
{
    public interface ICocktailJson
    {
        string Name { get; set; }
        string Description { get; set; }
        string[] Ingredient { get; set; }
    }
}
