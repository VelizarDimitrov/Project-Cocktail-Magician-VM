using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailCommentViewModel
    {
        public CocktailCommentViewModel(ICocktailComment cocktailComment)
        {
            UserName = cocktailComment.UserName;
            CocktailName = cocktailComment.CocktailName;
            Comment = cocktailComment.Comment;
            UserId = cocktailComment.UserId;
            CocktailId = cocktailComment.CocktailId;
            CreatedOn = cocktailComment.CreatedOn;
        }
        public string UserName { get; set; }
        public string CocktailName { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int CocktailId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
