using IProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IProject.Controllers
{
    public class FriendController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        public FriendController(ApplicationContext context,UserManager<User> userManager
            )
        {
            this._context = context;
            this._userManager = userManager;
        }
        public IActionResult Friends()
        {
            var currentUserId = _userManager.GetUserId(User);
            var friends = _context.Friends.Where(f => f.UserId == currentUserId).ToList();
            var itemFiles = _context.Files.ToList();
            var users = _context.Users.ToList();
            return View(Tuple.Create(itemFiles, friends,users));
        }
        public IActionResult ShowUserPage(string id)
        {
            //var currentUserId = _userManager.GetUserId(User);
            var users = _context.Users.Where(f=>f.Id==id).ToList();
            var itemFiles = _context.Files.Where(f=>f.UserId==id).ToList();
            var itemPhotoCovers = _context.PhotoCovers.Where(f => f.UserId == id).ToList();
            return View(Tuple.Create(itemFiles, itemPhotoCovers, users));
        }

        public async Task<IActionResult> AddFriend(string id)
        {
            var user = await _userManager.GetUserAsync(this.User);
            bool isFriend = false;
            foreach (var f in _context.Friends.Where(p=>p.UserId==user.Id))
            {
                if (id == f.UserFriendId)
                {
                    isFriend = true;
                }
            }
            if (isFriend==false)
            {
                var users = _context.Users.ToList();
                var u = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == user.Id);
                UserFriendShip ship = new UserFriendShip { UserId = user.Id, UserFriendId = id };
                _context.Friends.Add(ship);
                _context.SaveChanges();
            }
            return RedirectToAction("ShowAllUsers");
        }
        public IActionResult ShowAllUsers()
        {
            var currentUserId = _userManager.GetUserId(User);
            var users = _context.Users.ToList();
            var itemFiles = _context.Files.ToList();
            var itemPhotoCovers = _context.PhotoCovers.Where(f => f.UserId == currentUserId).ToList();
            return View(Tuple.Create(itemFiles, itemPhotoCovers, users));
        }
        //
    }
}
