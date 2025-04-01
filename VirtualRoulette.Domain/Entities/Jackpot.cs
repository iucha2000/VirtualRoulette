using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.ValueObjects;
using VirtualRoulette.Shared.Constants;

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
            Amount = Amount.Add(amount * NumberValues.UnitsPerCent);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddPercentageToJackpot(long amount, double percentage)
        {
            long amountInUnits = amount * NumberValues.UnitsPerCent;
            long amountToAdd = (long)(amountInUnits * percentage);

            Amount = Amount.Add(amountToAdd);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
