using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly UserManager<User> _userManager;
    public DeleteUserHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _userManager = userManager;
    }

    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.DeleteUserDto.UserId);

        if (await _userManager.CheckPasswordAsync(user, request.DeleteUserDto.UserPassword))
        {
            _repositoryWrapper.UserRepository.Delete(user);
        }

        var isSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        if (isSuccess)
        {
            return Result.Ok(Unit.Value);
        }
        
        return Result.Fail("error");
        
    }
}
