using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ge.singular.roulette;
using VirtualRoulette.Application.Interfaces.Services;

namespace VirtualRoulette.Infrastructure.Services
{
    public class BetAnalyzerService : IBetAnalyzerService
    {
        public bool IsBetValid(string bet)
        {
            var response = CheckBets.IsValid(bet);
            return response.getIsValid();
        }

        public long GetBetAmount(string bet)
        {
            var response = CheckBets.IsValid(bet);
            return response.getBetAmount();
        }

        public int EstimateBetWin(string bet, int winningNumber)
        {
            return CheckBets.EstimateWin(bet, winningNumber);
        }
    }
}
