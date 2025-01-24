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
using Streetcode.DAL.Entities.Users.Expertise;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringLocalizer<UserSharedResource> _localizer;

    public UpdateUserHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IStringLocalizer<UserSharedResource> localizer)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _localizer = localizer;
    }

    public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserEmail = HttpContextHelper.GetCurrentUserEmail(_httpContextAccessor);
            var user = await _userManager.Users
                .Include(u => u.Expertises)
                .SingleOrDefaultAsync(u => u.Email == currentUserEmail);

            if (user is null)
            {
                string errorMessage = _localizer["UserWithSuchUsernameNotExists"];
                _logger.LogError(request, errorMessage);
                return Result.Fail(errorMessage);
            }

            var toAddExpertises = await UpdateManyToManyRelationship(request, user);

            user.Expertises.AddRange(toAddExpertises);

            user.UserName = request.UserDto.UserName;
            user.Name = request.UserDto.Name;
            user.Surname = request.UserDto.Surname;
            user.AvatarId = request.UserDto.AvatarId;
            user.AboutYourself = request.UserDto.AboutYourself;
            user.PhoneNumber = request.UserDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError(request, string.Join(" ", errorMessages));
                return Result.Fail<UserDTO>(errorMessages);
            }

            await _repositoryWrapper.SaveChangesAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return Result.Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(request, ex.Message);
            return Result.Fail(ex.Message);
        }
    }

    private async Task<IEnumerable<Expertise>> UpdateManyToManyRelationship(UpdateUserCommand request, User user)
    {
        var requestedExpertiseIds = request.UserDto.Expertises.Select(e => e.Id).ToList();

        var toDelete = user.Expertises
            .Where(ue => !requestedExpertiseIds.Contains(ue.Id))
            .ToList();

        if (toDelete.Any())
        {
            foreach (var expertise in toDelete)
            {
                user.Expertises.Remove(expertise);
            }
        }

        var toAddExpertiseIds = request.UserDto.Expertises
            .Where(e => user.Expertises.All(ue => ue.Id != e.Id))
            .Select(e => e.Id)
            .ToList();

        var toAddExpertises = await _repositoryWrapper.ExpertiseRepository
            .GetAllAsync(e => toAddExpertiseIds.Contains(e.Id));
        return toAddExpertises;
    }
}