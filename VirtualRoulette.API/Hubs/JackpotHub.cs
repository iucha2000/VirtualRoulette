using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Security.Claims;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.API.Hubs
{
    [Authorize]
    public class JackpotHub : Hub
    {
        //Store user id with connections here
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

        //On connect, add userId to connections dictionary
        public override async Task OnConnectedAsync()
        {
            var userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context!.ConnectionId;
                await Groups.AddToGroupAsync(Context!.ConnectionId, TextValues.ConnectedUsersGroup);
            }

            await base.OnConnectedAsync();
        }

        //On disconnect, remove userId from connections dictionary
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections.TryRemove(userId, out _);
                await Groups.RemoveFromGroupAsync(Context!.ConnectionId, TextValues.ConnectedUsersGroup);
            }

            await base.OnDisconnectedAsync(exception);
        }

        //Get user connectionId by userId
        public static string? GetConnectionId(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }
    }
}
