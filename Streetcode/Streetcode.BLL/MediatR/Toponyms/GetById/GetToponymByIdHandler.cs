using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetById;

public class GetToponymByIdHandler : IRequestHandler<GetToponymByIdQuery, Result<ToponymDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetToponymByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<ToponymDTO>> Handle(GetToponymByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Toponym, bool>>? basePredicate = t => t.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, t => t.Streetcodes);

        var toponym = await _repositoryWrapper.ToponymRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (toponym is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyToponymWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<ToponymDTO>(toponym));
    }
}