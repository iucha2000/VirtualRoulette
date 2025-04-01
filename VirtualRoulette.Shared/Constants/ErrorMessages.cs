using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Shared.Constants
{
    public static class ErrorMessages
    {
        public const string RegistrationFailed = "User registration failed.";
        public const string LoginFailed = "User login failed.";
        public const string UserAlreadyExists = "User with given username already exists.";
        public const string UserNotFound = "User with given credentials does not exist.";
        public const string UserNotAuthenticated = "User is not authenticated, please log in again.";
        public const string NotEnoughBalance = "User has not enough balance for this operation.";
        public const string DatabaseUpdateError = "An error occurred while saving changes to the database.";
        public const string MinGreaterThanMax = "Min value can not be greater than Max value.";
        public const string JackpotNotFound = "Jackpot not found for this game session.";
        public const string MoneyCanNotBeNegative = "Money amount cannot be negative.";
    }
}
