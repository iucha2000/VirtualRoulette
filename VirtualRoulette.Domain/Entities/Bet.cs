using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Enums;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Domain.Entities
{
    public class Bet : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid SpinId { get; set; }
        public string BetDetails { get; set; }
        public Money BetAmount { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public BetStatus Status { get; set; }
        public int? WinningNumber { get; set; }
        public Money WonAmount { get; set; }

        public Bet() { }

        public Bet(Guid userId, Guid spinId, string details, string ipAddress) : base()
        {
            UserId = userId;
            SpinId = spinId;
            BetDetails = details;
            BetAmount = Money.Zero;
            IpAddress = ipAddress;
            CreatedAt = DateTime.UtcNow;
            Status = BetStatus.Pending;
            WinningNumber = null;
            WonAmount = Money.Zero;
        }

        public void MarkAsAccepted()
        {
            Status = BetStatus.Accepted;
        }

        public void MarkAsRejected()
        {
            Status = BetStatus.Rejected;
        }

        public void UpdateWinnings(long winAmount)
        {
            WonAmount = new Money(winAmount);
            Status = winAmount > 0 ? BetStatus.Won : BetStatus.Lost;
        }
    }
}
