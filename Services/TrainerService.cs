
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class TrainerService
    {
        private readonly WorkshopHubDbContext _context;

        public TrainerService(WorkshopHubDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<List<Trainer>> GetAllAsync()
        {
            return await _context.Trainers.ToListAsync();
        }

        // GET BY ID
        public async Task<Trainer?> GetByIdAsync(int id)
        {
            return await _context.Trainers.FindAsync(id);
        }

        // CREATE
        public async Task<Trainer> AddAsync(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return trainer;
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, Trainer updatedTrainer)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
                return false;

            trainer.FullName = updatedTrainer.FullName;
            trainer.Specialization = updatedTrainer.Specialization;
            trainer.PhoneNumber = updatedTrainer.PhoneNumber;
            trainer.Email = updatedTrainer.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
                return false;

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
