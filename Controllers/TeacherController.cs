using Microsoft.AspNetCore.Mvc;
using FacultyMonitoringSystem.Data;
using FacultyMonitoringSystem.Models;
using System;
using System.Linq;

namespace FacultyMonitoringSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        // ================= DASHBOARD =================
        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var data = _context.TimetableEntries
                .Where(t => t.TeacherId == userId)
                .ToList();

            return View(data);
        }

        // ================= ADD TIMETABLE =================
        public IActionResult AddTimetable()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTimetable(TimetableEntry entry)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            // 🔥 VALIDATION: TIME RANGE
            if (entry.StartTime < new TimeSpan(9, 30, 0) ||
                entry.EndTime > new TimeSpan(16, 30, 0))
            {
                ModelState.AddModelError("", "Time must be between 09:30 and 16:30");
                return View(entry);
            }

            // 🔥 VALIDATION: 1 HOUR SLOT
            if ((entry.EndTime - entry.StartTime).TotalMinutes != 60)
            {
                ModelState.AddModelError("", "Each lecture must be exactly 1 hour");
                return View(entry);
            }

            // DEBUG
            Console.WriteLine("ADD -> UserName: " + HttpContext.Session.GetString("UserName"));
            Console.WriteLine("ADD -> Department: " + HttpContext.Session.GetString("Department"));

            // SAVE USER INFO
            entry.TeacherId = userId.Value;
            entry.TeacherName = HttpContext.Session.GetString("UserName") ?? "Unknown";
            entry.Department = HttpContext.Session.GetString("Department") ?? "Not Set";

            // DEFAULT STATUS
            entry.Status = "Pending";
            entry.Remarks = "";

            _context.TimetableEntries.Add(entry);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            var entry = _context.TimetableEntries.Find(id);

            if (entry == null)
                return RedirectToAction("Dashboard");

            return View(entry);
        }

        [HttpPost]
        public IActionResult Edit(TimetableEntry entry)
        {
            var existing = _context.TimetableEntries.Find(entry.Id);

            if (existing != null)
            {
                // VALIDATION: TIME RANGE
                if (entry.StartTime < new TimeSpan(9, 30, 0) ||
                    entry.EndTime > new TimeSpan(16, 30, 0))
                {
                    ModelState.AddModelError("", "Time must be between 09:30 and 16:30");
                    return View(entry);
                }

                // VALIDATION: 1 HOUR SLOT
                if ((entry.EndTime - entry.StartTime).TotalMinutes != 60)
                {
                    ModelState.AddModelError("", "Each lecture must be exactly 1 hour");
                    return View(entry);
                }

                existing.Day = entry.Day;
                existing.StartTime = entry.StartTime;
                existing.EndTime = entry.EndTime;
                existing.Subject = entry.Subject;
                existing.Room = entry.Room;
                existing.Semester = entry.Semester ?? existing.Semester;
                existing.AcademicYear = entry.AcademicYear;

                // DEBUG
                Console.WriteLine("EDIT -> UserName: " + HttpContext.Session.GetString("UserName"));
                Console.WriteLine("EDIT -> Department: " + HttpContext.Session.GetString("Department"));

                // UPDATE NAME + DEPARTMENT
                existing.TeacherName = HttpContext.Session.GetString("UserName") ?? "Unknown";
                existing.Department = HttpContext.Session.GetString("Department") ?? "Not Set";

                // RESET AFTER EDIT
                existing.Status = "Pending";
                existing.Remarks = "";

                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            var entry = _context.TimetableEntries.Find(id);

            if (entry != null)
            {
                _context.TimetableEntries.Remove(entry);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // ================= VIEW MY TIMETABLE =================
        public IActionResult ViewMyTimetable()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var data = _context.TimetableEntries
                .Where(t => t.TeacherId == userId)
                .ToList();

            // 🔥 FIX: Calculate overall timetable status
            if (data.Count > 0)
            {
                if (data.All(t => t.Status == "Approved"))
                    ViewBag.Status = "Approved";
                else if (data.Any(t => t.Status == "Rejected"))
                    ViewBag.Status = "Rejected";
                else
                    ViewBag.Status = "Pending";
            }
            else
            {
                ViewBag.Status = "Not Available";
            }

            // OPTIONAL: Show name & department
            ViewBag.Name = HttpContext.Session.GetString("UserName");
            ViewBag.Department = HttpContext.Session.GetString("Department");

            return View(data);
        }
    }
}