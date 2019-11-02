using Data.SolutionPreLoad.JsonParsers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.SolutionPreLoad.JsonParsers
{
    public class UserJson : IUserJson
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string AccountType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
