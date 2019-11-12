using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
    interface IBarPhoto
    {
        int Id { get; set; }
        byte[] BarCover { get; set; }
        int BarId { get; set; }
        Bar Bar { get; set; }
    }
}
