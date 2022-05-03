using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public WalkDifficultyRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            // Create new Guid

            walkDifficulty.Id = new Guid();

            // Stage passed in object

            await _dbContext.WalkDifficulty.AddAsync(walkDifficulty);

            // Save object to DB

            await _dbContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            // Find the object

            var walkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

            // If doesn't exist, return null

            if (walkDifficulty == null)
            {
                return null;
            }

            // If exists, delete

            _dbContext.WalkDifficulty.Remove(walkDifficulty);

            // Save changes and return removed object to API

            await _dbContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _dbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            // Find and return object

            return await _dbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            // Find object

            var existingWalkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

            // If not found, return null

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            // If found, update fields and save entry in DB

            existingWalkDifficulty.Code = walkDifficulty.Code;
            await _dbContext.SaveChangesAsync();

            // Return updated object to API

            return existingWalkDifficulty;
        }
    }
}
