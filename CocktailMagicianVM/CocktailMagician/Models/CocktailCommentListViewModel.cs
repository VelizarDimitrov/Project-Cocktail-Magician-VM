using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailCommentListViewModel
    {
        public CocktailCommentListViewModel(ICollection<CocktailComment> cocktailComments)
        {
            this.CocktailComments = new List<CocktailCommentViewModel>();
            foreach (var cocktailComment in cocktailComments)
            {
                CocktailComments.Add(new CocktailCommentViewModel(cocktailComment));
            }
        }
        public List<CocktailCommentViewModel> CocktailComments { get; set; }
    }
}
