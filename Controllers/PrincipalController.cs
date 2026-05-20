using Microsoft.AspNetCore.Mvc;
using FacultyMonitoringSystem.Data;
using System;
using System.Linq;

namespace FacultyMonitoringSystem.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly AppDbContext _context;

        public PrincipalController(AppDbContext context)
        {
            _context = context;
        }

        // ================= SEARCH PAGE =================
        public IActionResult Search()
        {
            return View();
        }

        // ================= SEARCH RESULT =================
        [HttpPost]
        public IActionResult Search(string teacherName)
        {
            if (string.IsNullOrEmpty(teacherName))
            {
                ViewBag.Error = "Please enter teacher name";
                return View();
            }

            // ✅ SEARCH ONLY TEACHERS
            var teacher = _context.Users
                .FirstOrDefault(u =>
                    u.Role == "Teacher" &&
                    u.Name.ToLower().Contains(teacherName.ToLower()));

            if (teacher == null)
            {
                ViewBag.Error = "Teacher does not exist";
                return View();
            }

            var now = DateTime.Now;

            var currentTime = new TimeSpan(now.Hour, now.Minute, 0);
            var today = now.DayOfWeek.ToString();

            // ✅ FIND CURRENT CLASS
            var current = _context.TimetableEntries
                .Where(t =>
                    t.TeacherId == teacher.Id &&
                    t.Day.ToLower() == today.ToLower())
                .ToList()
                .FirstOrDefault(t =>
                    t.StartTime <= currentTime &&
                    t.EndTime >= currentTime
                );

            ViewBag.Teacher = teacher;
            ViewBag.Current = current;

            return View();
        }

        // ================= VIEW FULL TIMETABLE =================
        public IActionResult ViewTimetable(int id)
        {
            var data = _context.TimetableEntries
                .Where(t => t.TeacherId == id)
                .ToList();

            if (data == null || data.Count == 0)
                return RedirectToAction("Search");

            // ✅ BASIC INFO
            ViewBag.Name = data.First().TeacherName;
            ViewBag.Department = data.First().Department;

            // 🔥 FIXED STATUS LOGIC (MAIN FIX)
            if (data.All(e => e.Status == "Approved"))
            {
                ViewBag.Status = "Approved";
            }
            else if (data.Any(e => e.Status == "Rejected"))
            {
                ViewBag.Status = "Rejected";
            }
            else
            {
                ViewBag.Status = "Pending";
            }

            return View(data);
        }
    }
}