using Microsoft.EntityFrameworkCore.Update.Internal;
using NZWalks.API.NewFolder.NewFolder;

namespace NZWalks.API.Repositories
{ //will contain definition of methods that we want to expose
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllASync();

        Task<Region?> GetById(Guid id); //? means that null can be returned as well

        Task<Region> Create(Region region);

        Task<Region?> Update(Guid id, Region region);

        Task<Region?> Delete(Guid id);
    }
}
