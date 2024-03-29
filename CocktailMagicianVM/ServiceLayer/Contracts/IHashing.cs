﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Contracts
{
    public interface IHashing
    {
        string Hash(string password);

        bool Verify(string password, string hash);
    }
}
