using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Shared.Constants
{
    public static class TextValues
    {
        public const string JackpotHubPath = "/jackpothub";
        public const string ConnectedUsersGroup = "ConnectedUsers";

        public const string ConnectMessage = "Subscribed to jackpot updates";
        public const string DisconnectMessage = "Unsubscribed from jackpot updates";
        public const string JackpotUpdateMessage = "ReceiveJackpotUpdate";

        public const string AllowAllCors = "AllowAllCors";

    }
}
