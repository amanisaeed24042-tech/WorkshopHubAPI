
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;

namespace WorkshopHubAPI.Services
{
    public class DashboardService
    {
        private readonly WorkshopHubDbContext _db;

        public DashboardService(WorkshopHubDbContext db)
        {
            _db = db;
        }

        public async Task<object> GetStatsAsync()
        {
            int totalWorkshops = await _db.Workshops.CountAsync();

            int totalTrainers = await _db.Trainers.CountAsync();

            int totalTrainees = await _db.Trainees.CountAsync();

            int totalRegistrations = await _db.Registrations
                .CountAsync(r => r.Status == "Confirmed");

            int totalSessions = await _db.WorkshopSessions.CountAsync();

            int totalPresent = await _db.Attendances
                .CountAsync(a => a.Status == "Present");

            int totalAbsent = await _db.Attendances
                .CountAsync(a => a.Status == "Absent");

            int totalAttendanceRecords = totalPresent + totalAbsent;

            double attendanceRate = totalAttendanceRecords == 0
                ? 0
                : Math.Round(
                    (double)totalPresent / totalAttendanceRecords * 100,
                    2);

            return new
            {
                TotalWorkshops = totalWorkshops,
                TotalTrainers = totalTrainers,
                TotalTrainees = totalTrainees,
                TotalRegistrations = totalRegistrations,
                TotalSessions = totalSessions,
                TotalPresent = totalPresent,
                TotalAbsent = totalAbsent,
                AttendanceRate = attendanceRate
            };
        }

        public async Task<object> GetUpcomingWorkshopsAsync()
        {
            var today = DateTime.Today;

            return await _db.Workshops
                .AsNoTracking()
                .Where(w => w.StartDate >= today)
                .OrderBy(w => w.StartDate)
                .Select(w => new
                {
                    w.WorkshopId,
                    w.Title,
                    w.StartDate,
                    w.EndDate,
                    w.Capacity,
                    TrainerName = w.Trainer.FullName
                })
                .Take(5)
                .ToListAsync();
        }
    }
}
