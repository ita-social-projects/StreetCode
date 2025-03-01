using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetById;

public class GetSubtitleByIdHandler : IRequestHandler<GetSubtitleByIdQuery, Result<SubtitleDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetSubtitleByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<SubtitleDTO>> Handle(GetSubtitleByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.AdditionalContent.Subtitle, bool>> basePredicate = sub => sub.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, sub => sub.Streetcode);

        var subtitle = await _repositoryWrapper.SubtitleRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (subtitle is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindSubtitleWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            var returner = Result.Fail(new Error(errorMsg));
            return returner;
        }

        return Result.Ok(_mapper.Map<SubtitleDTO>(subtitle));
    }
}