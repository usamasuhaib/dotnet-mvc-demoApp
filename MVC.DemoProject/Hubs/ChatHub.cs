using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MVC.DemoProject.Models;

namespace MVC.DemoProject.Hubs
{
    public class ChatHub :Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendMessage( string message)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            var userName = user.Email;

            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}
