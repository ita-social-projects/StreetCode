using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.GetByName;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.GetByUserName;

public class GetOtherUserByUserNameHandler : IRequestHandler<GetOtherUserByUserNameQuery, Result<UserProfileDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringLocalizer<UserSharedResource> _localizer;

    public GetOtherUserByUserNameHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IStringLocalizer<UserSharedResource> localizer)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _localizer = localizer;
    }

    public async Task<Result<UserProfileDTO>> Handle(GetOtherUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.UserName == HttpContextHelper.GetCurrentUserName(_httpContextAccessor), include: qu => qu.Include(x => x.Expertises));

        if (user is null)
        {
            string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
            _logger.LogError(request, errorMessage);
            return Result.Fail(errorMessage);
        }

        var userDto = _mapper.Map<UserProfileDTO>(user);
        userDto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        return Result.Ok(userDto);
    }
}