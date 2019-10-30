using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface IUserBar
    {
        int UserId { get; set; }
        User User { get; set; }
        int BarId { get; set; }
        Bar Bar { get; set; }
    }
}
