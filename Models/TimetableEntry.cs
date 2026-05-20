using System;

namespace FacultyMonitoringSystem.Models
{
    public class TimetableEntry
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        // 👇 ADD THIS (fix for HOD view)
        

        public string TeacherName { get; set; } = "";
        public string Department { get; set; } = "";

        public string Day { get; set; } = "";

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Subject { get; set; } = "";

        public string Room { get; set; } = "";

        public string Semester { get; set; } = "";

        public string AcademicYear { get; set; } = "";

        public string Status { get; set; } = "Pending";

        public string Remarks { get; set; } = "";
    }
}