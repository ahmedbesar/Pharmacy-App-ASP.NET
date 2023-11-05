using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace Pharmacy.Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessageToOwner(string message)
        {
            // Send the message to the owner
            await Clients.Group("owner").SendAsync("ReceiveMessage", "Owner", message);
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            // Send the message to the specified user
            await Clients.User(userId).SendAsync("ReceiveMessage", "Owner", message);
        }

        public async Task JoinAsOwner()
        {
            // Allow the connection to join the "owner" group
            await Groups.AddToGroupAsync(Context.ConnectionId, "owner");
        }
    }
}