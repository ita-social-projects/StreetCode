using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;

public record GetStreetcodeByIndexQuery(int Index, UserRole? userRole)
    : IRequest<Result<StreetcodeDTO>>;
