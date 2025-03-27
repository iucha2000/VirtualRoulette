using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public static Money Zero => new(0);

        public Money(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Money amount cannot be negative.");
            }
                
            Amount = amount;
        }

        public Money Add(decimal amount) => new(Amount + amount);
        public Money Subtract(decimal amount) => new(Amount - amount);
    }
}
