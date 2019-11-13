using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class AddBarViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }     
        public string Description { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

    }
}
