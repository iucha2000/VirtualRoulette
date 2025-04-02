using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.DTOs
{
    public class ExceptionDto
    {
        public int Status { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
    }
}
