using Data.SolutionPreLoad.JsonParsers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.SolutionPreLoad.JsonParsers
{
    public class CocktailJson : ICocktailJson
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Ingredients { get; set; }
        public string PhotoPath { get; set; }
    }
}
