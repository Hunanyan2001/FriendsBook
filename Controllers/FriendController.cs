using IProject.Models;
using IProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IProject.Controllers
{
    public class FriendController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public FriendController(ApplicationContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public IActionResult Friends(string id)
        {
            //var currentUserId = _userManager.GetUserId(User);
            //if (id == null)
            //{
            //    id = currentUserId;
            //}
            AllUsersViewModel model = new AllUsersViewModel()
            {
                Photo = _context.Files.Where(f => f.UserId == id).ToList(),
                Friends = _context.Friends.Where(f => f.UserId == id).ToList(),
                Users = _context.Users.Where(f => f.Id == id).ToList(),
                CoversPhoto = _context.PhotoCovers.Where(f => f.UserId == id).ToList()
            };
            return View(model);
        }

        public IActionResult ShowUserPage(string id)
        {
            AllUsersViewModel userPage = new AllUsersViewModel()
            {
                //Users = _context.Users.Include(f=>f.FileModels).Where(f => f.Id == id).ToList(),
                Users = _context.Users.Where(f => f.Id == id).ToList(),
                Photo = _context.Files.Where(f => f.UserId == id).ToList(),
                CoversPhoto = _context.PhotoCovers.Where(f => f.UserId == id).ToList(),
                Friends = _context.Friends.ToList()
            };
            return View(userPage);
        }

        public async Task<IActionResult> AddFriend(string id)
        {
            var user = await _userManager.GetUserAsync(this.User);
            bool isFriend = false;
            foreach (var f in _context.Friends.Where(p => p.UserId == user.Id))
            {
                if (id == f.UserFriendId)
                {
                    isFriend = true;
                }
            }
            if (isFriend == false)
            {
                var avatar = _context.Files.ToList();
                var users = _context.Users.ToList();
                var u = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == user.Id);
                UserFriendShip ship = new UserFriendShip
                {
                    UserId = user.Id,
                    UserFriendId = id,
                    UserFriendEmail = users.Where(p => p.Id == id).Last().Email,
                    FriendAvatar = avatar.Where(p => p.UserId == id).Last().Path
                };
                
                _context.Friends.Add(ship);
                _context.SaveChanges();
            }
            return RedirectToAction("ShowAllUsers");
        }
        public async Task<IActionResult> RemoveFriend(string id)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var avatar = _context.Files.ToList();
            var users = _context.Users.ToList();
            var u = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == user.Id);
            var friend = _context.Friends.Find(user.Id,id);
            if (friend == null)
            {
                return NotFound();
            }
            _context.Friends.Remove(friend);
            _context.SaveChanges();
            id = _userManager.GetUserId(User);
            return RedirectToAction("Friends", new { @id = id });
        }

        //[HttpPost]
        public IActionResult ShowAllUsers()
        {
            var currentUserId = _userManager.GetUserId(User);
            AllUsersViewModel ghostUserPage = new AllUsersViewModel()
            {
                Users = _context.Users.ToList(),
                Photo = _context.Files.ToList(),
                CoversPhoto = _context.PhotoCovers.ToList(),
                Friends = _context.Friends.ToList()
            };
            return View(ghostUserPage);
        }
    }
}
