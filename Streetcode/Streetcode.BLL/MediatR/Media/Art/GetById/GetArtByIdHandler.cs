using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Art.GetById;

public class GetArtByIdHandler : IRequestHandler<GetArtByIdQuery, Result<ArtDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetArtByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<ArtDTO>> Handle(GetArtByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Media.Images.Art, bool>>? basePredicate = f => f.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, a => a.Streetcode);

        var art = await _repositoryWrapper.ArtRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (art is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnArtWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<ArtDTO>(art));
    }
}