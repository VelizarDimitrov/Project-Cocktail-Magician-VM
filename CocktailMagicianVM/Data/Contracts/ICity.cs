using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface ICity
    {
        int Id { get; set; }
        string Name { get; set; }
        Country Country { get; set; }
        ICollection<Bar> Bars { get; set; }
    }
}
