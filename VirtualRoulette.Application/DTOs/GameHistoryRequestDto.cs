﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.DTOs
{
    public class GameHistoryRequestDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
