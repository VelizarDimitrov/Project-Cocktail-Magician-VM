using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Areas.Magician.Models
{
    public class AddBarViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
    }
}
