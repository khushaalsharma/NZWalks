using NZWalks.API.Models.DTOs;
using NZWalks.API.NewFolder.NewFolder;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);

        Task<Walk?> GetById(Guid id);

        Task<Walk?> UpdateWalk(Guid id, Walk walk);

        Task<Walk?> DeleteWalk(Guid id);
    }
}
