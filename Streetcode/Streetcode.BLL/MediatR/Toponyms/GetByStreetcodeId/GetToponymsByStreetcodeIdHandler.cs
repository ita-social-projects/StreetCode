﻿using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

public class GetToponymsByStreetcodeIdHandler : IRequestHandler<GetToponymsByStreetcodeIdQuery, Result<IEnumerable<ToponymDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetToponymsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ToponymDTO>>> Handle(GetToponymsByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var toponyms = await _repositoryWrapper
            .ToponymRepository
            .GetAllAsync(
                predicate: sc => sc.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                include: scl => scl
                    .Include(sc => sc.Coordinate));
        toponyms.DistinctBy(x => x.StreetName);
        if (toponyms is null)
        {
            return Result.Fail(new Error($"Cannot find any toponym by the streetcode id: {request.StreetcodeId}"));
        }

        var toponymDto = _mapper.Map<IEnumerable<ToponymDTO>>(toponyms);
        return Result.Ok(toponymDto);
    }
}