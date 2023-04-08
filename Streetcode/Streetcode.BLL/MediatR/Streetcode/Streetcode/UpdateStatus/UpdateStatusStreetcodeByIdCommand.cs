using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;

public record UpdateStatusStreetcodeByIdCommand(int Id, StreetcodeStatus Status) : IRequest<Result<Unit>>;