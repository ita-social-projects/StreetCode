using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Users.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Users;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public UpdateUserValidator(
        BaseUserValidator baseUserValidator,
        IRepositoryWrapper repositoryWrapper,
        IStringLocalizer<UserSharedResource> localizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(dto => dto.UserDto).SetValidator(baseUserValidator);

        RuleFor(dto => dto.UserDto)
            .MustAsync(BeUniqueUserName)
            .WithMessage(localizer["UserWithSuchUsernameExists"]);
    }

    private async Task<bool> BeUniqueUserName(UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var existingUserByUserName = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(n => n.UserName == dto.UserName);

        if (existingUserByUserName is not null)
        {
            return existingUserByUserName.Email == dto.Email;
        }

        return true;
    }
}