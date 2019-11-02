using System;
using System.Collections.Generic;
using System.Text;

namespace Data.SolutionPreLoad.JsonParsers.Contracts
{
    public interface IUserJson
    {
         string UserName { get; set; }
         string FirstName { get; set; }
         string LastName { get; set; }
         string Password { get; set; }
         string AccountType { get; set; }
         string Country { get; set; }
         string City { get; set; }
    }
}
