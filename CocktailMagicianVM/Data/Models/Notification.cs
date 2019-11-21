using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Notification : INotification
    {
        public Notification()
        {
            SentOn = DateTime.Now;
            Seen = 0;
        }

        public int id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime SentOn { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public byte Seen { get; set; }
    }
}
