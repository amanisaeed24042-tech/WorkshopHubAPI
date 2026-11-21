
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class TraineeService
    {
        private readonly WorkshopHubDbContext _context;

        public TraineeService(WorkshopHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trainee>> GetAllAsync()
        {
            return await _context.Trainees
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Trainee?> GetByIdAsync(int id)
        {
            return await _context.Trainees
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TraineeId == id);
        }

        public async Task<Trainee> AddAsync(TraineeRequest request)
        {
            var trainee = new Trainee
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };

            _context.Trainees.Add(trainee);
            await _context.SaveChangesAsync();

            return trainee;
        }

        public async Task<bool> UpdateAsync(
            int id, TraineeRequest request)
        {
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null)
                return false;

            trainee.FullName = request.FullName;
            trainee.PhoneNumber = request.PhoneNumber;
            trainee.Email = request.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var trainee = await _context.Trainees
                .Include(t => t.Registrations)
                .FirstOrDefaultAsync(t => t.TraineeId == id);

            if (trainee == null)
                return false;

            if (trainee.Registrations.Any())
                throw new InvalidOperationException(
                    "Cannot delete a trainee with registrations.");

            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
