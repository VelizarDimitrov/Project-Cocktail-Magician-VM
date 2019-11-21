using Data.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Magician.Models
{
    public class EditBarViewModel
    {
        public EditBarViewModel()
        {

        }
        public EditBarViewModel(IBar bar)
        {
            Name = bar.Name;
            Description = bar.Description;
            Address = bar.Address;
            Country = bar.Country.Name;
            City = bar.City.Name;
            Id = bar.Id;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
    }
}
