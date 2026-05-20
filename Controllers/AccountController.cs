using Microsoft.AspNetCore.Mvc;
using FacultyMonitoringSystem.Data;
using FacultyMonitoringSystem.Models;
using System.Linq;

namespace FacultyMonitoringSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN PAGE =================
        public IActionResult Login()
        {
            return View();
        }

        // ================= SIGNUP PAGES =================
        public IActionResult TeacherSignup() => View();
        public IActionResult HODSignup() => View();
        public IActionResult PrincipalSignup() => View();

        // ================= REGISTER =================
        [HttpPost]
        public IActionResult Register(User user)
        {
            // 🔥 DEBUG (VERY IMPORTANT)
            System.Console.WriteLine("REGISTER NAME: " + user.Name);
            System.Console.WriteLine("REGISTER DEPT: " + user.Department);
            System.Console.WriteLine("REGISTER SEMESTER: " + user.Semester);
            System.Console.WriteLine("REGISTER ROLE: " + user.Role);

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input";
                return View("Login");
            }

            // OPTIONAL PASSWORD CHECK
            if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 8)
            {
                ViewBag.Error = "Password must be at least 8 characters";
                return View("Login");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ================= LOGIN POST =================
        [HttpPost]
        public IActionResult Login(string email, string password, string role)
        {
            var user = _context.Users
                .FirstOrDefault(u =>
                    u.Email == email &&
                    u.Password == password &&
                    u.Role == role);

            if (user != null)
            {
                // ✅ STORE SESSION DATA
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name ?? "");
                HttpContext.Session.SetString("Department", user.Department ?? "");
                HttpContext.Session.SetString("Role", user.Role ?? "");

                // 🔥 OPTIONAL (if you want semester later)
                HttpContext.Session.SetString("Semester", user.Semester ?? "");

                // 🔥 DEBUG
                System.Console.WriteLine("LOGIN NAME: " + user.Name);
                System.Console.WriteLine("LOGIN SEM: " + user.Semester);

                // ROLE REDIRECT
                if (user.Role == "Teacher")
                    return RedirectToAction("Dashboard", "Teacher");

                if (user.Role == "HOD")
                    return RedirectToAction("Dashboard", "HOD");

                if (user.Role == "Principal")
                    return RedirectToAction("Search", "Principal");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }
    }
}