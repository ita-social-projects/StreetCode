using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.GetByName;

public class GetByUserNameHandler : IRequestHandler<GetByUserNameQuery, Result<UserDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<UserSharedResource> _localizer;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetByUserNameHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IStringLocalizer<UserSharedResource> localizer)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _localizer = localizer;
    }

    public async Task<Result<UserDTO>> Handle(GetByUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.Email == HttpContextHelper.GetCurrentUserEmail(_httpContextAccessor), include: qu => qu.Include(x => x.Expertises));

        if (user is null)
        {
            string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
            _logger.LogError(request, errorMessage);
            return Result.Fail(errorMessage);
        }

        var userDto = _mapper.Map<UserDTO>(user);
        userDto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        return Result.Ok(userDto);
    }
}