using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class Country:ICountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Bar> Bars { get; set; }
    }
}
