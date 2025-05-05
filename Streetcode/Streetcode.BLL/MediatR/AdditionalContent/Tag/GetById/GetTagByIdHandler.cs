using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;

public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Result<TagDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTagByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<TagDTO>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.AdditionalContent.Tag, bool>>? basePredicate = t => t.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, t => t.Streetcodes);

        var tag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (tag is null)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindTagWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<TagDTO>(tag));
    }
}
