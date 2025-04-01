using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Domain.ValueObjects
{
    public class Money
    {
        public long CentAmount { get; private set; }
        public static Money Zero => new(0);

        public Money(long centAmount)
        {
            if (centAmount < 0)
            {
                throw new ArgumentException(ErrorMessages.MoneyCanNotBeNegative);
            }
                
            CentAmount = centAmount;
        }

        public Money Add(long centAmount) => new(CentAmount + centAmount);
        public Money Subtract(long centAmount) => new(CentAmount - centAmount);
    }
}
