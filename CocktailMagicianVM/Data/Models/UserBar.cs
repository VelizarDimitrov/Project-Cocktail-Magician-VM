using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class UserBar:IUserBar
    {
     public  int UserId { get; set; }
     public  User User { get; set; }
     public  int BarId { get; set; }
     public  Bar Bar { get; set; }
    }
}
