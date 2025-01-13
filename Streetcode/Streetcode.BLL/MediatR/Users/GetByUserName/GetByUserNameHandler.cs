using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.GetByName;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.GetByUserName;

public class GetByUserNameHandler : IRequestHandler<GetByUserNameQuery, Result<UserProfileDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly UserManager<User> _userManager;

    public GetByUserNameHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _userManager = userManager;
    }

    public async Task<Result<UserProfileDTO>> Handle(GetByUserNameQuery request, CancellationToken cancellationToken)
    {
        //var user = await _userManager.FindByIdAsync(request.UserName);
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.UserName == request.UserName, include: qu => qu.Include(x => x.Expertises));

        if (user == null)
        {
            return Result.Fail("User not found.");
        }

        var userDto = _mapper.Map<UserProfileDTO>(user);

        return Result.Ok(userDto);
    }
}