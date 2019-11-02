using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.JasonParsers.Contracts
{
    public interface IBarJson
    {
        string Name { get; set; }
        string Address { get; set; }
        string Description { get; set; }
        string Country { get; set; }
        string City { get; set; }
        string BarCoverPath { get; set; }
    }
}
