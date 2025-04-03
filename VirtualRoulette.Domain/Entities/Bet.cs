using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Enums;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Domain.Entities
{
    //Bet entity to store user bet info
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

        public Bet(Guid userId, Guid spinId, string details, Money betAmount, string ipAddress, DateTime createdAt) : base()
        {
            UserId = userId;
            SpinId = spinId;
            BetDetails = details;
            BetAmount = betAmount;
            IpAddress = ipAddress;
            CreatedAt = createdAt;
            Status = BetStatus.Accepted;
            WinningNumber = null;
            WonAmount = Money.Zero;
        }

        //Update winning number and win amount for user bet
        public void UpdateWinnings(int winningNumber, long wonAmount)
        {
            WinningNumber = winningNumber;
            WonAmount = new Money(wonAmount);
            Status = wonAmount > 0 ? BetStatus.Won : BetStatus.Lost;
        }
    }
}
