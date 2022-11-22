using IProject.Models;
using IProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;


namespace IProject.Controllers
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public ChatHub(ApplicationContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }
        public async Task Send(string message, string sender,string receiver)
        {
            await Clients.All.SendAsync("Send", message,sender,receiver);
            Conversation conversation = new Conversation
            {
                sender_id = sender,
                receiver_id = receiver,
                message = message,
            };
            _context.Add(conversation);
            _context.SaveChanges();
        }
    }
}