using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Domain.ValueObjects
{
    public class BetDetails
    {
        public string BetJson { get; private set; }

        public BetDetails(string betJson)
        {
            if (string.IsNullOrWhiteSpace(betJson))
            {
                throw new ArgumentException("Bet details cannot be empty.");
            }
                
            BetJson = betJson;
        }
    }
}
