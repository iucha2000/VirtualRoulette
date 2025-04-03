using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ge.singular.roulette;
using VirtualRoulette.Application.Interfaces.Services;

namespace VirtualRoulette.Infrastructure.Services
{
    //BetAnalyzerSevice to encapsulate external ge.singular.roulette library logic in out service
    public class BetAnalyzerService : IBetAnalyzerService
    {
        //Check if bet is valid and return bool value
        public bool IsBetValid(string bet)
        {
            var response = CheckBets.IsValid(bet);
            return response.getIsValid();
        }

        //Get bet amount (long) from provided string
        public long GetBetAmount(string bet)
        {
            var response = CheckBets.IsValid(bet);
            return response.getBetAmount();
        }

        //Estimate bet win (in cents) based on provided bet and winning number
        public int EstimateBetWin(string bet, int winningNumber)
        {
            return CheckBets.EstimateWin(bet, winningNumber);
        }
    }
}
