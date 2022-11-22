using IProject.Models;
using IProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IProject.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        public ChatController(ApplicationContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }
        public ActionResult AllChat()
        {
            var currentUserId = _userManager.GetUserId(User);
            AllUsersViewModel model = new AllUsersViewModel()
            {
                Photo = _context.Files.Where(f => f.UserId == currentUserId).ToList(),
                Friends = _context.Friends.Where(f => f.UserId == currentUserId).ToList(),
                Users = _context.Users.Where(f => f.Id == currentUserId).ToList(),
                CoversPhoto = _context.PhotoCovers.Where(f => f.UserId == currentUserId).ToList()
            };
            return View(model);
        }

        public ActionResult Chat(string id)
        {
            var currentUserId = _userManager.GetUserId(User);
            AllUsersViewModel model = new AllUsersViewModel()
            {
                Users = _context.Users.Where(f => f.Id == currentUserId).ToList(),
                Friends = _context.Friends.Where(f => f.UserFriendId == id && f.UserId == currentUserId).ToList(),
                Chats =_context.Chats.Where(f=>f.sender_id == currentUserId && f.receiver_id == id).ToList(),
            };
            return View(model);
        }
    }
}
