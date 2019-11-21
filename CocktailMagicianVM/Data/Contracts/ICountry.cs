using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface ICountry
    {
        int Id { get; set; }
        string Name { get; set; }
        ICollection<City> Cities { get; set; }
        ICollection<Bar> Bars { get; set; }
    }
}
