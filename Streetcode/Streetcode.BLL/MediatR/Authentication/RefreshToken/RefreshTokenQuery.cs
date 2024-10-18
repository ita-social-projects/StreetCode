﻿using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.RefreshToken;

namespace Streetcode.BLL.MediatR.Authentication.RefreshToken
{
    public record RefreshTokenQuery(RefreshTokenRequestDTO token)
        : IRequest<Result<RefreshTokenResponceDTO>>;
}
