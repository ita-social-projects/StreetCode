using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;

public record GetTagByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<TagDTO>>;
