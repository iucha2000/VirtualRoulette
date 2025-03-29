using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Domain.Exceptions
{
    public class NotEnoughBalanceException : Exception
    {
        public NotEnoughBalanceException(string message) : base(message) { }
    }
}
