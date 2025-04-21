using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle;

public class GetTagByTitleHandler : IRequestHandler<GetTagByTitleQuery, Result<TagDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTagByTitleHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByTitleQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.AdditionalContent.Tag, bool>>? basePredicate = f => f.Title == request.Title;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, t => t.Streetcodes);

        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (tag is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyTagByTheTitle", request.Title].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<TagDTO>(tag));
    }
}
