using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.GetById;

public class GetFactByIdHandler : IRequestHandler<GetFactByIdQuery, Result<FactDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetFactByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<FactDto>> Handle(GetFactByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>? basePredicate = f => f.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, f => f.Streetcode);

        var facts = await _repositoryWrapper.FactRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (facts is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindFactWithCorrespondingCategoryId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<FactDto>(facts));
    }
}