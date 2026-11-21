
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class WorkshopService
    {
        private readonly WorkshopHubDbContext _context;

        public WorkshopService(WorkshopHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Workshop>> GetAllAsync(
            string? search)
        {
            var query = _context.Workshops
                .Include(w => w.Trainer)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(w =>
                    w.Title.Contains(search));
            }

            return await query.ToListAsync();
        }

        public async Task<Workshop?> GetByIdAsync(int id)
        {
            return await _context.Workshops
                .Include(w => w.Trainer)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.WorkshopId == id);
        }

        private async Task ValidateAsync(
            WorkshopRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new InvalidOperationException(
                    "Workshop title is required.");

            if (request.EndDate < request.StartDate)
                throw new InvalidOperationException(
                    "End date must not be before start date.");

            var trainerExists = await _context.Trainers
                .AnyAsync(t => t.TrainerId == request.TrainerId);

            if (!trainerExists)
                throw new InvalidOperationException(
                    "Trainer does not exist.");
        }

        public async Task<Workshop> AddAsync(
            WorkshopRequest request)
        {
            await ValidateAsync(request);

            var workshop = new Workshop
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Capacity = request.Capacity,
                Price = request.Price,
                TrainerId = request.TrainerId
            };

            _context.Workshops.Add(workshop);
            await _context.SaveChangesAsync();

            return workshop;
        }

        public async Task<bool> UpdateAsync(
            int id, WorkshopRequest request)
        {
            var workshop = await _context.Workshops.FindAsync(id);

            if (workshop == null)
                return false;

            await ValidateAsync(request);

            var confirmedCount = await _context.Registrations
                .CountAsync(r =>
                    r.WorkshopId == id &&
                    r.Status == "Confirmed");

            if (request.Capacity < confirmedCount)
                throw new InvalidOperationException(
                    "Capacity cannot be less than confirmed registrations.");

            workshop.Title = request.Title;
            workshop.Description = request.Description;
            workshop.StartDate = request.StartDate;
            workshop.EndDate = request.EndDate;
            workshop.Capacity = request.Capacity;
            workshop.Price = request.Price;
            workshop.TrainerId = request.TrainerId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workshop = await _context.Workshops.FindAsync(id);

            if (workshop == null)
                return false;

            bool hasSessions = await _context.WorkshopSessions
                .AnyAsync(s => s.WorkshopId == id);

            bool hasRegistrations = await _context.Registrations
                .AnyAsync(r => r.WorkshopId == id);

            if (hasSessions || hasRegistrations)
                throw new InvalidOperationException(
                    "Cannot delete a workshop with sessions or registrations.");

            _context.Workshops.Remove(workshop);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
