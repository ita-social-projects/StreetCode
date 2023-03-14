using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
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

        if (streetcodes is null || streetcodes.Count() == 0)
        {
            return;
        }

        foreach (var streetcode in streetcodes)
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