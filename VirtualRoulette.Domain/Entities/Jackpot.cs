using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Domain.Entities
{
    public class Jackpot : BaseEntity
    {
        public Money Amount { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Jackpot() : base()
        {
            Amount = Money.Zero;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddToJackpot(long amount)
        {
            Amount = Amount.Add(amount);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
