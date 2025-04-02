using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Shared.Constants
{
    public static class ClientMessages
    {
        public const string BetIsNotValid = "Bet is not in valid format.";
        public const string UserHasNotEnoughBalance = "User has not enough balance for this bet.";
        public const string YouWonText = "Congratulations! you won.";
        public const string YouLostText = "You lost, try again!";
        public const string DepositFundsSuccess = "Funds successfully deposited to balance.";
        public const string WithdrawFundsSuccess = "Funds successfully withdrawn from balance.";
        public const string SignOutSuccess = "User signed out successfully.";
    }
}
