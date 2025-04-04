﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Shared.Constants
{
    //NumberValues static class to store constant values of different number values used in solution
    public static class NumberValues
    {
        public const int RouletteMinValue = 0;
        public const int RouletteMaxValue = 36;
        public const int InactivityPeriod = 5;
        public const int InactivityCheckPeriod = 1;
        public const long InitialJackpotValue = 100000;
        public const double JackpotIncreasePercentage = 0.1;
        public const long UnitsPerCent = 10_000;
    }
}
