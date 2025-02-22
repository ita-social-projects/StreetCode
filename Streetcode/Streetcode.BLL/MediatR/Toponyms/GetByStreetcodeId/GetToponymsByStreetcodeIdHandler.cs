using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

public class GetToponymsByStreetcodeIdHandler : IRequestHandler<GetToponymsByStreetcodeIdQuery, Result<IEnumerable<ToponymDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

    public GetToponymsByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerNo = stringLocalizerNo;
    }

    public async Task<Result<IEnumerable<ToponymDTO>>> Handle(GetToponymsByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var toponyms = await _repositoryWrapper
            .ToponymRepository
            .GetAllAsync(
                predicate: sc => sc.Streetcodes.Any(s => s.Id == request.StreetcodeId),
                include: scl => scl
                    .Include(sc => sc.Coordinate!));
        toponyms = toponyms.DistinctBy(x => x.StreetName).ToList();

        if (!toponyms.Any())
        {
            string errorMsg = _stringLocalizerNo["NoToponymWithSuchId", request.StreetcodeId].Value;
            _logger.LogInformation(errorMsg);
            return Result.Ok(Enumerable.Empty<ToponymDTO>());
        }

        var toponymDto = toponyms.GroupBy(x => x.StreetName).Select(group => group.First()).Select(x => _mapper.Map<ToponymDTO>(x));

        return Result.Ok(toponymDto);
    }
}
