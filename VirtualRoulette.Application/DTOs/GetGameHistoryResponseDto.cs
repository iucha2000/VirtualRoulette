using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Application.DTOs
{
    public class GetGameHistoryResponseDto
    {
        public List<GameHistoryEntry> Entries { get; set; } = new List<GameHistoryEntry>();
    }

    public class GameHistoryEntry
    {
        public Guid SpinId { get; set; }
        public long BetAmount { get; set; }
        public long WonAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
