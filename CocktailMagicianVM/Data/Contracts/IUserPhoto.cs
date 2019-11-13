using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface IUserPhoto
    {
        int Id { get; set; }
        byte[] UserCover { get; set; }
        int UserId { get; set; }
        User User { get; set; }
    }
}
