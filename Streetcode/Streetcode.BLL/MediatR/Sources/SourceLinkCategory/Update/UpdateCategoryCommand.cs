﻿using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public record UpdateCategoryCommand(UpdateSourceLinkCategoryDTO Category)
        : IRequest<Result<UpdateSourceLinkCategoryDTO>>;
}
