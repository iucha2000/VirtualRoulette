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
        public BetDetails BetDetails { get; set; }
        public Money BetAmount { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public BetStatus Status { get; set; }
        public int? WinningNumber { get; set; }
        public Money WinAmount { get; set; }

        public Bet() { }

        public Bet(Guid userId, BetDetails details, Money amount, string ipAddress) : base()
        {
            UserId = userId;
            SpinId = Guid.NewGuid();
            BetDetails = details;
            BetAmount = amount;
            IpAddress = ipAddress;
            CreatedAt = DateTime.UtcNow;
            Status = BetStatus.Pending;
            WinningNumber = null;
            WinAmount = Money.Zero;
        }

        public void MarkAsAccepted()
        {
            Status = BetStatus.Accepted;
        }

        public void MarkAsRejected()
        {
            Status = BetStatus.Rejected;
        }

        public void UpdateWinnings(decimal winAmount)
        {
            WinAmount = new Money(winAmount);
            Status = winAmount > 0 ? BetStatus.Won : BetStatus.Lost;
        }
    }
}
