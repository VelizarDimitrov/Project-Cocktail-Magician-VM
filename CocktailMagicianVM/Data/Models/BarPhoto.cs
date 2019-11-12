using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class BarPhoto : IBarPhoto
    {
        public int Id { get; set; }
        public byte[] BarCover { get; set; }
        public int BarId { get; set; }
        public Bar Bar { get; set; }
    }
}
