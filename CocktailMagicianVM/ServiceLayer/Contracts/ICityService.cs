﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
   public interface ICityService
    {
        Task CreateCityAsync(string cityName, string countryName);
    }
}
