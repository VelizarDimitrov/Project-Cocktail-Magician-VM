﻿using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class CocktailComment : ICocktailComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CocktailId { get; set; }
        public Cocktail Cocktail { get; set; }
        public string CocktailName { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
