using CLI.Core.JasonParsers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.JasonParsers
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
