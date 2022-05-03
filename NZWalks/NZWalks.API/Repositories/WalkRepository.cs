using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _DbContext;

        public WalkRepository(NZWalksDbContext dbContext)
        {
            this._DbContext = dbContext;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign new ID

            walk.Id = new Guid();

            // Create new walk
            
            await _DbContext.Walks.AddAsync(walk);

            // Save changes to DB

            await _DbContext.SaveChangesAsync();

            // Return new walk difficulty to API controller

            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await _DbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }

            _DbContext.Walks.Remove(existingWalk);

            await _DbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _DbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            var walk = await _DbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);

            return walk;

        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _DbContext.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.Name = walk.Name;
                existingWalk.Length = walk.Length;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await _DbContext.SaveChangesAsync();
                return existingWalk;
            }

            return null;
        }
    }
}
