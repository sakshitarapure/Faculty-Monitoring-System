using Microsoft.AspNetCore.Mvc;
using FacultyMonitoringSystem.Data;
using System.Linq;

namespace FacultyMonitoringSystem.Controllers
{
    public class HODController : Controller
    {
        private readonly AppDbContext _context;

        public HODController(AppDbContext context)
        {
            _context = context;
        }

        // ================= DASHBOARD =================
        public IActionResult Dashboard()
        {
            var data = _context.TimetableEntries
                .GroupBy(t => t.TeacherId)
                .Select(g => g.First())
                .ToList();

            return View(data);
        }

        // ================= VIEW TIMETABLE (GRID) =================
        public IActionResult ViewTimetable(int id)
        {
            var data = _context.TimetableEntries
                .Where(t => t.TeacherId == id)
                .ToList();

            ViewBag.Name = data.FirstOrDefault()?.TeacherName;
            ViewBag.Department = data.FirstOrDefault()?.Department;

            if (data.All(e => e.Status == "Approved"))
                ViewBag.Status = "Approved";
            else if (data.Any(e => e.Status == "Rejected"))
                ViewBag.Status = "Rejected";
            else
                ViewBag.Status = "Pending";

            return View(data);
        }

        // ================= REVIEW (GET) =================
        public IActionResult Review(int id)
        {
            var data = _context.TimetableEntries
                .Where(t => t.TeacherId == id)
                .ToList();

            if (data.Count == 0)
                return RedirectToAction("Dashboard");

            ViewBag.TeacherId = id;
            ViewBag.Name = data.First().TeacherName;
            ViewBag.Department = data.First().Department;

            return View(); // ✅ NO MODEL (clean page)
        }

        // ================= REVIEW (POST) =================
        [HttpPost]
        public IActionResult Review(int teacherId, string status, string remarks)
        {
            var entries = _context.TimetableEntries
                .Where(t => t.TeacherId == teacherId)
                .ToList();

            foreach (var entry in entries)
            {
                entry.Status = status;
                entry.Remarks = remarks;
            }

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}