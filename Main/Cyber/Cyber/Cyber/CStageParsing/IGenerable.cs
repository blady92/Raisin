﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public interface IGenerable
    {
        bool[,] Structure { get; set; }
        IGenerable clone();
        bool IsSingleBlock();
    }
}
