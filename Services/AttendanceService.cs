
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class AttendanceService
    {
        private readonly WorkshopHubDbContext _db;

        public AttendanceService(WorkshopHubDbContext db)
        {
            _db = db;
        }

        public async Task<List<Attendance>> GetBySessionAsync(
            int sessionId)
        {
            return await _db.Attendances
                .AsNoTracking()
                .Where(a =>
                    a.WorkshopSessionId == sessionId)
                .ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _db.Attendances
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.AttendanceId == id);
        }

        private async Task ValidateAsync(
            AttendanceRequest request)
        {
            if (request.Status != "Present" &&
                request.Status != "Absent")
                throw new InvalidOperationException(
                    "Status must be Present or Absent.");

            var registration = await _db.Registrations
                .FindAsync(request.RegistrationId);

            var session = await _db.WorkshopSessions
                .FindAsync(request.WorkshopSessionId);

            if (registration == null || session == null)
                throw new InvalidOperationException(
                    "Registration or session not found.");

            if (registration.Status != "Confirmed")
                throw new InvalidOperationException(
                    "Registration is not confirmed.");

            if (registration.WorkshopId != session.WorkshopId)
                throw new InvalidOperationException(
                    "Registration and session belong to different workshops.");
        }

        public async Task<Attendance> AddAsync(
            AttendanceRequest request)
        {
            await ValidateAsync(request);

            bool exists = await _db.Attendances
                .AnyAsync(a =>
                    a.RegistrationId == request.RegistrationId &&
                    a.WorkshopSessionId == request.WorkshopSessionId);

            if (exists)
                throw new InvalidOperationException(
                    "Attendance already recorded.");

            var attendance = new Attendance
            {
                RegistrationId = request.RegistrationId,
                WorkshopSessionId = request.WorkshopSessionId,
                Status = request.Status,
                Notes = request.Notes
            };

            _db.Attendances.Add(attendance);
            await _db.SaveChangesAsync();

            return attendance;
        }

        public async Task<bool> UpdateAsync(
            int id, AttendanceRequest request)
        {
            var attendance = await _db.Attendances
                .FindAsync(id);

            if (attendance == null)
                return false;

            await ValidateAsync(request);

            bool duplicate = await _db.Attendances
                .AnyAsync(a =>
                    a.AttendanceId != id &&
                    a.RegistrationId == request.RegistrationId &&
                    a.WorkshopSessionId == request.WorkshopSessionId);

            if (duplicate)
                throw new InvalidOperationException(
                    "Attendance already exists.");

            attendance.RegistrationId = request.RegistrationId;
            attendance.WorkshopSessionId = request.WorkshopSessionId;
            attendance.Status = request.Status;
            attendance.Notes = request.Notes;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await _db.Attendances
                .FindAsync(id);

            if (attendance == null)
                return false;

            _db.Attendances.Remove(attendance);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
