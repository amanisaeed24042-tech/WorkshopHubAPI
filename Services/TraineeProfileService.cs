
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.DTOs;
using WorkshopHubAPI.Models;

namespace WorkshopHubAPI.Services
{
    public class TraineeProfileService
    {
        private readonly WorkshopHubDbContext _db;

        public TraineeProfileService(WorkshopHubDbContext db)
        {
            _db = db;
        }

        public async Task<TraineeProfile?> GetByTraineeIdAsync(int traineeId)
        {
            return await _db.TraineeProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.TraineeId == traineeId);
        }

        public async Task<TraineeProfile> AddAsync(
            TraineeProfileRequest request)
        {
            var traineeExists = await _db.Trainees
                .AnyAsync(t => t.TraineeId == request.TraineeId);

            if (!traineeExists)
                throw new InvalidOperationException(
                    "Trainee not found.");

            var profileExists = await _db.TraineeProfiles
                .AnyAsync(p => p.TraineeId == request.TraineeId);

            if (profileExists)
                throw new InvalidOperationException(
                    "Trainee already has a profile.");

            var profile = new TraineeProfile
            {
                TraineeId = request.TraineeId,
                Address = request.Address,
                EducationLevel = request.EducationLevel,
                Notes = request.Notes
            };

            _db.TraineeProfiles.Add(profile);
            await _db.SaveChangesAsync();

            return profile;
        }

        public async Task<bool> UpdateAsync(
            int traineeId,
            TraineeProfileRequest request)
        {
            var profile = await _db.TraineeProfiles
                .FirstOrDefaultAsync(p => p.TraineeId == traineeId);

            if (profile == null)
                return false;

            if (request.TraineeId != traineeId)
                throw new InvalidOperationException(
                    "Trainee ID cannot be changed.");

            profile.Address = request.Address;
            profile.EducationLevel = request.EducationLevel;
            profile.Notes = request.Notes;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int traineeId)
        {
            var profile = await _db.TraineeProfiles
                .FirstOrDefaultAsync(p => p.TraineeId == traineeId);

            if (profile == null)
                return false;

            _db.TraineeProfiles.Remove(profile);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
