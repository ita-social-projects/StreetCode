﻿using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById;

public class GetFactByIdHandler : IRequestHandler<GetFactByIdQuery, Result<FactDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetFactByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<FactDTO>> Handle(GetFactByIdQuery request, CancellationToken cancellationToken)
    {
        var facts = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (facts is null)
        {
            return Result.Fail(new Error($"Cannot find any fact with corresponding id: {request.Id}"));
        }

        var factsDto = _mapper.Map<FactDTO>(facts);
        return Result.Ok(factsDto);
    }
}