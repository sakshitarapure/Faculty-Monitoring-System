using System;

namespace FacultyMonitoringSystem.Models
{
    public class TimetableUpload
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        // ✅ ADD THIS (VERY IMPORTANT)
        public User? Teacher { get; set; }

        public string FilePath { get; set; } = "";

        public string Status { get; set; } = "";

        public string Remarks { get; set; } = "";

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}