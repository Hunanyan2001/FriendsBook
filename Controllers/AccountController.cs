using IProject.Models;
using IProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IProject.Controllers
{

    //User@ vor ira nkarenr@ tena mekel urish User vor gnuma hyur ira avatar@ cuyc ta
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment,
            ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
            _context = context;
        }

        public IActionResult PageUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = _userManager.GetUserId(User);
                AllUsersViewModel userPage = new AllUsersViewModel()
                {
                    Users = _context.Users.Where(f => f.Id == currentUserId).ToList(),
                    Photo = _context.Files.Where(f => f.UserId == currentUserId).ToList(),
                    CoversPhoto = _context.PhotoCovers.Where(f => f.UserId == currentUserId).ToList(),
                    Friends = _context.Friends.ToList()
                };
                return View(userPage);
            }
            else
            {
                return View("Register");
            }
        }
        public IActionResult PhotoCovers(string id)
        {
            var currentUserId = _userManager.GetUserId(User);
            AllUsersViewModel photoCovers = new AllUsersViewModel()
            {
                Users = _context.Users.Where(f=>f.Id == id).ToList(),
                Photo = _context.Files.Where(f => f.UserId == id).ToList(),
                CoversPhoto = _context.PhotoCovers.Where(f => f.UserId == id).ToList(),
                Friends = _context.Friends.ToList()
            };
            return View(photoCovers);
        }
        public IActionResult ShowAllPhoto(string id)
        {
            var photoGhost = _context.Files.Where(f => f.UserId == id).ToList();
           // var currentUserId = _userManager.GetUserId(User);
            AllUsersViewModel allPhoto = new AllUsersViewModel()
            {
                Users = _context.Users.Where(f=>f.Id == id).ToList(),
                Photo = _context.Files.Where(f => f.UserId == id).ToList(),
                CoversPhoto = _context.PhotoCovers.Where(f => f.UserId == id).ToList(),
                Friends = _context.Friends.ToList()
            };
            return View(allPhoto);
        }

        [HttpGet]
        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModels model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PageUser(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path, UserId = userId };
                _context.Files.Add(file);
                _context.SaveChanges();
            }
            return RedirectToAction("PageUser");
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> PhotoCovers(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                PhotoCovers file = new PhotoCovers { Name = uploadedFile.FileName, Path = path, UserId = userId };
                _context.PhotoCovers.Add(file);
                _context.SaveChanges();
            }
            return RedirectToAction("PageUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("PageUser", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
