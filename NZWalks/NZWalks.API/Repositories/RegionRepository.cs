using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {

        private readonly NZWalksDbContext _DbContext;

        public RegionRepository(NZWalksDbContext DbContext)
        {
            this._DbContext = DbContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _DbContext.Regions.ToListAsync();
        }
    }
}
