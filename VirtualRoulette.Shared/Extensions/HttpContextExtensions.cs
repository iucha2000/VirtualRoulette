using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Shared.Extensions
{
    //HttpContextExtensions to get UserId and UserIpAddress from given httpContext
    public static class HttpContextExtensions
    {
        //Get userId from httpContext. If null return Guid.Empty
        public static Guid GetUserId(this HttpContext context)
        {
            var userIdClaim = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            return Guid.Empty;
        }

        //get user IP address from httpContext. If null, return "Unknown IP" value
        public static string GetUserIpAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        }
    }
}
