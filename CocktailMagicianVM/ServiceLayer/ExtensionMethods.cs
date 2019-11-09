using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    public static class ExtensionMethods
    {
        public static bool IsBetween( this Double value, int lower, int higher)
        {
            if (value >= lower && value <= higher)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
