using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.GetByName;

public class GetByUserIdHandler : IRequestHandler<GetByUserIdQuery, Result<UserDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly UserManager<User> _userManager;

    public GetByUserIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind, UserManager<User> userManager)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _userManager = userManager;
    }

    public async Task<Result<UserDTO>> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
    {
        //var user = await _userManager.FindByIdAsync(request.UserName);
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.UserId, include: qu => qu.Include(x => x.Expertises));

        if (user == null)
        {
            return Result.Fail("User not found.");
        }

        var userDto = _mapper.Map<UserDTO>(user);
        userDto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        return Result.Ok(userDto);
    }
}