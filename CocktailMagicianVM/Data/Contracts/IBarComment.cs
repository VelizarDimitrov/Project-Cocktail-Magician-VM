using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface IBarComment
    {
        int Id { get; set; }
        string Comment { get; set; }
        int UserId { get; set; }
        User User { get; set; }
        int BarId { get; set; }
        Bar Bar { get; set; }
        string BarName { get; set; }
        string UserUserName { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
