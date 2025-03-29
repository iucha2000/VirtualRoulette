using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Enums;

namespace VirtualRoulette.Application.DTOs
{
    public class MakeBetResponseDto
    {
        public BetStatus Status { get; set; }
        public Guid? SpinId { get; set; }
        public int? WinningNumber { get; set; }
        public int? WonAmount { get; set; }
    }
}
