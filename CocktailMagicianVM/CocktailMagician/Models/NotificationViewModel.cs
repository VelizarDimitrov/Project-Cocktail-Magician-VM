using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class NotificationViewModel
    {
        public NotificationViewModel(INotification notification)
        {
            this.Id = notification.id;
            this.Name = notification.Name;
            this.Text = notification.Text;
            this.SentOn = notification.SentOn;
            this.Seen = notification.Seen;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime SentOn { get; set; }
        public byte Seen { get; set; }
    }
}
