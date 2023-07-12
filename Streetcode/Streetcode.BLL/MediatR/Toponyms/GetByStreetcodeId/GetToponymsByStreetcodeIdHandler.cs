using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

public class GetToponymsByStreetcodeIdHandler : IRequestHandler<GetToponymsByStreetcodeIdQuery, Result<IEnumerable<ToponymDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetToponymsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
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
            string errorMsg = $"Cannot find any toponym by the streetcode id: {request.StreetcodeId}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var toponymDto = toponyms.GroupBy(x => x.StreetName).Select(group => group.First()).Select(x => _mapper.Map<ToponymDTO>(x));
        return Result.Ok(toponymDto);
    }
}
