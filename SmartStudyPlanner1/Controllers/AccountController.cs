using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;
using System.Security.Cryptography;
using System.Text;

namespace SmartStudyPlanner1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) { _db = db; }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var hashed = HashPassword(password);
            var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == hashed);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.FullName ?? "User");
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_db.Users.Any(u => u.Email == user.Email))
            {
                ViewBag.Error = "Email already exists. Please use a different email.";
                return View();
            }
            user.Password = HashPassword(user.Password);
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
