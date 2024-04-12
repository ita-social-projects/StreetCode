﻿using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create
{
    public record CreateTermCommand(TermCreateDTO Term) : IRequest<Result<TermDTO>>;
}
