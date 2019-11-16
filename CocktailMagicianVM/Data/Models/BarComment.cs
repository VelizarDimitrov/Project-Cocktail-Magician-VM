using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class BarComment:IBarComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BarId { get; set; }
        public Bar Bar { get; set; }
        public string BarName { get; set; }
        public string UserUserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
