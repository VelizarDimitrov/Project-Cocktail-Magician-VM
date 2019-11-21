using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class City:ICity
    {
        public City()
        {
            Bars = new List<Bar>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<Bar> Bars { get; set; }
    }
}
