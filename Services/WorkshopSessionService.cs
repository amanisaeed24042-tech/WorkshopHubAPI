
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class WorkshopSessionService
    {
        private readonly WorkshopHubDbContext _db;

        public WorkshopSessionService(WorkshopHubDbContext db)
        {
            _db = db;
        }

        public async Task<List<WorkshopSession>> GetAllAsync(
            int? workshopId)
        {
            var query = _db.WorkshopSessions
                .AsNoTracking().AsQueryable();

            if (workshopId.HasValue)
                query = query.Where(s =>
                    s.WorkshopId == workshopId.Value);

            return await query.OrderBy(s => s.SessionDate)
                .ToListAsync();
        }

        public async Task<WorkshopSession?> GetByIdAsync(int id)
        {
            return await _db.WorkshopSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.WorkshopSessionId == id);
        }

        private async Task ValidateAsync(
            WorkshopSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Topic))
                throw new InvalidOperationException(
                    "Session topic is required.");

            if (request.EndTime <= request.StartTime)
                throw new InvalidOperationException(
                    "End time must be after start time.");

            var workshop = await _db.Workshops
                .FindAsync(request.WorkshopId);

            if (workshop == null)
                throw new InvalidOperationException(
                    "Workshop not found.");

            if (request.SessionDate.Date <
                    workshop.StartDate.Date ||
                request.SessionDate.Date >
                    workshop.EndDate.Date)
                throw new InvalidOperationException(
                    "Session date must be within workshop dates.");
        }

        public async Task<WorkshopSession> AddAsync(
            WorkshopSessionRequest request)
        {
            await ValidateAsync(request);

            var session = new WorkshopSession
            {
                WorkshopId = request.WorkshopId,
                SessionDate = request.SessionDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Topic = request.Topic
            };

            _db.WorkshopSessions.Add(session);
            await _db.SaveChangesAsync();

            return session;
        }

        public async Task<bool> UpdateAsync(
            int id, WorkshopSessionRequest request)
        {
            var session = await _db.WorkshopSessions
                .FindAsync(id);

            if (session == null)
                return false;

            await ValidateAsync(request);

            bool hasAttendance = await _db.Attendances
                .AnyAsync(a => a.WorkshopSessionId == id);

            if (hasAttendance &&
                session.WorkshopId != request.WorkshopId)
                throw new InvalidOperationException(
                    "Cannot change workshop after attendance exists.");

            session.WorkshopId = request.WorkshopId;
            session.SessionDate = request.SessionDate;
            session.StartTime = request.StartTime;
            session.EndTime = request.EndTime;
            session.Topic = request.Topic;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await _db.WorkshopSessions
                .FindAsync(id);

            if (session == null)
                return false;

            if (await _db.Attendances.AnyAsync(a =>
                a.WorkshopSessionId == id))
                throw new InvalidOperationException(
                    "Cannot delete session with attendance.");

            _db.WorkshopSessions.Remove(session);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
