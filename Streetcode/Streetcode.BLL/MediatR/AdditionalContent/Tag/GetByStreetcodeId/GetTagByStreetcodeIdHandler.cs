using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;

public class GetTagByStreetcodeIdHandler : IRequestHandler<GetTagByStreetcodeIdQuery, Result<IEnumerable<StreetcodeTagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTagByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
    }

    public async Task<Result<IEnumerable<StreetcodeTagDTO>>> Handle(GetTagByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<StreetcodeContent, bool>>? basePredicate = str => str.Id == request.StreetcodeId;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole);

        var isStreetcodeExists = await _repositoryWrapper.StreetcodeRepository.FindAll(predicate: predicate).AnyAsync(cancellationToken);

        if (!isStreetcodeExists)
        {
            string errorMsg = _stringLocalizerCannotFind["CannotFindAnyStreetcodeWithCorrespondingId", request.StreetcodeId].Value;
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var tagIndexed = await _repositoryWrapper.StreetcodeTagIndexRepository
            .GetAllAsync(
                t => t.StreetcodeId == request.StreetcodeId,
                include: q => q.Include(t => t.Tag!));

        if (!tagIndexed.Any())
        {
            string message = "Returning empty enumerable of tags";
            _logger.LogInformation(message);
            return Result.Ok(Enumerable.Empty<StreetcodeTagDTO>());
        }

        return Result.Ok(_mapper.Map<IEnumerable<StreetcodeTagDTO>>(tagIndexed.OrderBy(ti => ti.Index)));
    }
}
