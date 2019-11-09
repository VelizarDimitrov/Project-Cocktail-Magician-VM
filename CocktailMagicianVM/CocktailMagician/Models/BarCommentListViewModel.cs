using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarCommentListViewModel
    {
        public BarCommentListViewModel(ICollection<BarComment> barComments)
        {
            this.BarComments = new List<BarCommentViewModel>();
            foreach (var barComment in barComments)
            {
                BarComments.Add(new BarCommentViewModel(barComment));
            }
        }
        public List<BarCommentViewModel> BarComments { get; set; }
    }
}
