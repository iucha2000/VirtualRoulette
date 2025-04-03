using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using VirtualRoulette.API.Hubs;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.API.Services
{
    //Service to interact with JackpotHub
    public class JackpotHubService : IJackpotHubService
    {
        private readonly IHubContext<JackpotHub> _hubContext;

        public JackpotHubService(IHubContext<JackpotHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //Connect user to JackpotHub, add to group
        public async Task ConnectUser(Guid userId)
        {
            await _hubContext.Clients.User(userId.ToString()).SendAsync(TextValues.ConnectMessage);
        }

        //Disconnect user from JackpotHub, remove from group
        public async Task DisconnectUser(Guid userId)
        {
            await _hubContext.Clients.User(userId.ToString()).SendAsync(TextValues.DisconnectMessage);

            var connectionId = JackpotHub.GetConnectionId(userId.ToString());
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Groups.RemoveFromGroupAsync(connectionId, TextValues.ConnectedUsersGroup);
            }
        }

        //Push new jackpot amount to all connected clients in the group
        public async Task PushJackpotUpdate(decimal newJackpotAmount)
        {
            await _hubContext.Clients.Group(TextValues.ConnectedUsersGroup).SendAsync(TextValues.JackpotUpdateMessage, newJackpotAmount);
        }
    }
}
