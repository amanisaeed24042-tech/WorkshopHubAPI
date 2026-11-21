
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class RegistrationService
    {
        private readonly WorkshopHubDbContext _db;

        public RegistrationService(WorkshopHubDbContext db)
        {
            _db = db;
        }

        public async Task<List<Registration>> GetAllAsync(
            int? workshopId)
        {
            var query = _db.Registrations
                .AsNoTracking().AsQueryable();

            if (workshopId.HasValue)
                query = query.Where(r =>
                    r.WorkshopId == workshopId.Value);

            return await query.ToListAsync();
        }

        public async Task<Registration?> GetByIdAsync(int id)
        {
            return await _db.Registrations
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.RegistrationId == id);
        }

        public async Task<Registration> AddAsync(
            RegistrationRequest request)
        {
            var trainee = await _db.Trainees
                .FindAsync(request.TraineeId);

            if (trainee == null)
                throw new InvalidOperationException(
                    "Trainee not found.");

            var workshop = await _db.Workshops
                .FindAsync(request.WorkshopId);

            if (workshop == null)
                throw new InvalidOperationException(
                    "Workshop not found.");

            var existing = await _db.Registrations
                .AnyAsync(r =>
                    r.TraineeId == request.TraineeId &&
                    r.WorkshopId == request.WorkshopId);

            if (existing)
                throw new InvalidOperationException(
                    "Trainee already registered.");

            var count = await _db.Registrations
                .CountAsync(r =>
                    r.WorkshopId == request.WorkshopId &&
                    r.Status == "Confirmed");

            if (count >= workshop.Capacity)
                throw new InvalidOperationException(
                    "Workshop is full.");

            var registration = new Registration
            {
                TraineeId = request.TraineeId,
                WorkshopId = request.WorkshopId,
                RegistrationDate = DateTime.UtcNow,
                Status = "Confirmed"
            };

            _db.Registrations.Add(registration);

            await _db.SaveChangesAsync();
            return registration;
        }

        public async Task<bool> CancelAsync(int id)
        {
            var registration = await _db.Registrations
                .FindAsync(id);

            if (registration == null)
                return false;

            if (registration.Status == "Cancelled")
                return true;

            if (await _db.Attendances.AnyAsync(a =>
                a.RegistrationId == id))
                throw new InvalidOperationException(
                    "Cannot cancel registration with attendance.");

            registration.Status = "Cancelled";

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var registration = await _db.Registrations
                .FindAsync(id);

            if (registration == null)
                return false;

            if (await _db.Attendances.AnyAsync(a =>
                a.RegistrationId == id))
                throw new InvalidOperationException(
                    "Cannot delete registration with attendance.");

            _db.Registrations.Remove(registration);

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
