using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Util.MappingResolvers;

public class UserNameResolver : IValueResolver<StreetcodeContent, StreetcodeDto, string>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UserNameResolver(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string Resolve(StreetcodeContent source, StreetcodeDto destination, string destMember, ResolutionContext context)
    {
        var user = _repositoryWrapper.UserRepository.GetSingleOrDefaultAsync(u => u.Id == source.UserId).Result;
        return user?.UserName;
    }
}