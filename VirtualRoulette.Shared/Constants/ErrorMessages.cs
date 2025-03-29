using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Shared.Constants
{
    public static class ErrorMessages
    {
        public const string UserAlreadyExists = "User with given username already exists.";
        public const string UserNotFound = "User with given credentials does not exist.";
        public const string UserNotAuthenticated = "Authenticated user not found.";
        public const string UserHasNotEnoughBalance = "User has not enough balance for this bet amount.";
        public const string DatabaseUpdateError = "An error occurred while saving changes to the database.";
    }
}
