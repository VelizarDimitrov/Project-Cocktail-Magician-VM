using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            lockedMessage = false;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool lockedMessage { get; set; }
    }
}
