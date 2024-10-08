﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.NewFolder.NewFolder;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository //will inject this repo in program.cs file
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Region>> GetAllASync()
        {
            return await dbContext.Regions.ToListAsync();

        }

        public async Task<Region?> GetById(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Region> Create(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> Update(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null) 
            {
                return null;
            }

            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> Delete(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id);

            if(existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
