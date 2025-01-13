using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.BLL.SharedResource;
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

    public UpdateUserHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _userManager = userManager;
    }

    public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.Expertises)
            .SingleOrDefaultAsync(u => u.UserName == request.UserDto.UserName);


        if (user == null)
        {
            return Result.Fail<UserDTO>(_stringLocalizerCannotFind["UserNotFound"]);
        }

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
            .Where(e => !user.Expertises.Any(ue => ue.Id == e.Id))
            .Select(e => e.Id)
            .ToList();

        var toAddExpertises = await _repositoryWrapper.ExpertiseRepository
            .GetAllAsync(e => toAddExpertiseIds.Contains(e.Id));

        user.Expertises.AddRange(toAddExpertises);

        user.Name = request.UserDto.Name;
        user.Surname = request.UserDto.Surname;
        user.AvatarId = request.UserDto.AvatarId;
        user.AboutYourself = request.UserDto.AboutYourself;
        user.PhoneNumber = request.UserDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Fail<UserDTO>(result.Errors.Select(e => e.Description).ToList());
        }

        await _repositoryWrapper.SaveChangesAsync();

        return Result.Ok(_mapper.Map<UserDTO>(user));

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }
}