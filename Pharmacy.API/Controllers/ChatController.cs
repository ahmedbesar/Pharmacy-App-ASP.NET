using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pharmacy.Infrastructure.Hubs;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpPost("owner")]
        public async Task<IActionResult> SendMessageToOwner([FromBody] string message)
        {
            await _hubContext.Clients.Group("owner").SendAsync("ReceiveMessage", "User", message);
            return Ok();
        }

        [Authorize]
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> SendMessageToUser(string userId, [FromBody] string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", "Owner", message);
            return Ok();
        }
    }
}
