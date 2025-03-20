using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetById;

public class GetTextByIdHandler : IRequestHandler<GetTextByIdQuery, Result<TextDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

    public GetTextByIdHandler(
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

    public async Task<Result<TextDTO>> Handle(GetTextByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<DAL.Entities.Streetcode.TextContent.Text, bool>>? basePredicate = f => f.Id == request.Id;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, t => t.Streetcode);

        var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(predicate: predicate);

        if (text is null)
        {
            var errorMessage = _stringLocalizerCannotFind["CannotFindAnyTextWithCorrespondingId", request.Id].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        return Result.Ok(_mapper.Map<TextDTO>(text));
    }
}