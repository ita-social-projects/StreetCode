using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.WebApi.Utils;

public class SoftDeletingUtils
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public SoftDeletingUtils(IRepositoryWrapper repository)
    {
        _repositoryWrapper = repository;
    }

    public async Task DeleteStreetcodeWithStageDeleted(int daysLife)
    {
        var streetcodes = await _repositoryWrapper.StreetcodeRepository
            .GetAllAsync(
            predicate: s => s.Status == StreetcodeStatus.Deleted && s.UpdatedAt.AddDays(daysLife) <= DateTime.Now,
            include: s => s.Include(x => x.Observers)
                           .Include(x => x.Targets));

        var streetcodeContents = streetcodes.ToArray();

        if (!streetcodeContents.Any())
        {
            return;
        }

        foreach (var streetcode in streetcodeContents)
        {
            _repositoryWrapper.StreetcodeRepository.Delete(streetcode);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if (!resultIsSuccess)
            {
                throw new Exception("Failed to delete a streetcode");
            }
        }
    }
}
