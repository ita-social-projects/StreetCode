﻿using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Team;

namespace Streetcode.BLL.MediatR.Team.Position.GetAll
{
    public record GetAllWithTeamMembersQuery : IRequest<Result<IEnumerable<PositionDTO>>>;
}
