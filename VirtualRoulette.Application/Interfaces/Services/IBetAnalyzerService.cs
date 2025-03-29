using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.Interfaces.Services
{
    public interface IBetAnalyzerService
    {
        bool IsBetValid(string bet);
        long GetBetAmount(string bet);
        int EstimateBetWin(string bet, int winningNumber);
    }
}
