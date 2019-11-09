using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarCommentViewModel
    {
        public BarCommentViewModel(IBarComment barComment)
        {
            UserName = barComment.User.UserName;
            BarName = barComment.Bar.Name;
            Comment = barComment.Comment;
            UserId = barComment.UserId;
            BarId = barComment.BarId;
        }
        public string UserName { get; set; }
        public string BarName { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int BarId { get; set; }
    }
}
