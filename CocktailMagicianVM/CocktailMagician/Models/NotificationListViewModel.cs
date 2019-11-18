using Data.Contracts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class NotificationListViewModel
    {
        public NotificationListViewModel(ICollection<Notification> notifications)
        {
            this.Notifications = new List<NotificationViewModel>();
            foreach (var item in notifications)
            {
                Notifications.Add(new NotificationViewModel(item));
            }
        }

        public List<NotificationViewModel> Notifications { get; set; }
    }
}
