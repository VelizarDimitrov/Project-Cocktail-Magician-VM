using CLI.Core.JasonParsers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.JasonParsers
{
    public class BarJson : IBarJson
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string BarCoverPath { get; set; }
    }
}
