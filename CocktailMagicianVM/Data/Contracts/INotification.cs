using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface INotification
    {
        int id { get; set; }

        string Name { get; set; }
        string Text { get; set; }

        DateTime SentOn { get; set; }

        int UserId { get; set; }

        User User { get; set; }

        byte Seen { get; set; }
    }
}
