using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Users.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Extensions;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Users;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public UpdateUserValidator(
        BaseUserValidator baseUserValidator,
        IRepositoryWrapper repositoryWrapper,
        IStringLocalizer<UserSharedResource> userLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(dto => dto.UserDto).SetValidator(baseUserValidator);

        RuleFor(dto => dto.UserDto.Id)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Id"]])
            .MustAsync(UserExists).WithMessage(userLocalizer["UserNotFound"]);

        RuleFor(dto => dto.UserDto)
            .MustAsync(BeUniqueUserName)
            .WhenAsync((dto, cancellation) => UserExists(dto.UserDto.Id, cancellation))
            .WithMessage(userLocalizer["UserWithSuchUsernameExists"]);
    }

    private async Task<bool> UserExists(string userId, CancellationToken cancellationToken)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(u => u.Id == userId);

        return user is not null;
    }

    private async Task<bool> BeUniqueUserName(UpdateUserDTO dto, CancellationToken cancellationToken)
    {
        var existingUserByUserName = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(n => n.UserName == dto.UserName.RemoveWhiteSpaces());

        if (existingUserByUserName is not null)
        {
            return existingUserByUserName.Id == dto.Id;
        }

        return true;
    }
}