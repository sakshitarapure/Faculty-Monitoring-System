using Microsoft.EntityFrameworkCore;
using FacultyMonitoringSystem.Models;

namespace FacultyMonitoringSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ✅ Users Table
        public DbSet<User> Users { get; set; }

        // ✅ Uploaded Files Table
        public DbSet<TimetableUpload> TimetableUploads { get; set; }

        // ✅ Timetable Entries (for real-time tracking)
        public DbSet<TimetableEntry> TimetableEntries { get; set; }
    }
}