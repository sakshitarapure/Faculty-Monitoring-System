using Microsoft.AspNetCore.Mvc;
using FacultyMonitoringSystem.Data;
using System;
using System.Linq;

namespace FacultyMonitoringSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string teacherName, string department)
        {
            // ✅ Current Day + Time
            string currentDay = DateTime.Now.DayOfWeek.ToString();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            var result = (from t in _context.TimetableEntries
                          join u in _context.Users on t.TeacherId equals u.Id
                          where u.Name.ToLower().Contains(teacherName.ToLower())
                                && u.Department == department
                                && t.Day == currentDay
                                && t.StartTime <= currentTime
                                && t.EndTime >= currentTime
                                && t.Status == "Approved"   // VERY IMPORTANT
                          select new
                          {
                              Teacher = u.Name,
                              Room = t.Room,
                              Subject = t.Subject
                          }).FirstOrDefault();

            ViewBag.Result = result;

            return View();
        }
    }
}