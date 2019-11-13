using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class UserPhoto:IUserPhoto
    {
        public int Id { get; set; }
        public byte[] UserCover { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
