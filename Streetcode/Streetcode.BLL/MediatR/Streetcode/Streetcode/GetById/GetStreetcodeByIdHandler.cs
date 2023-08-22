using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;

public class GetStreetcodeByIdHandler : IRequestHandler<GetStreetcodeByIdQuery, Result<StreetcodeDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetStreetcodeByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<StreetcodeDTO>> Handle(GetStreetcodeByIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
            predicate: st => st.Id == request.Id);

        if (streetcode is null)
        {
            string errorMsg = $"Cannot find any streetcode with corresponding id: {request.Id}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        var tagIndexed = await _repositoryWrapper.StreetcodeTagIndexRepository
                                        .GetAllAsync(
                                            t => t.StreetcodeId == request.Id,
                                            include: q => q.Include(ti => ti.Tag));
        var streetcodeDto = _mapper.Map<StreetcodeDTO>(streetcode);
        streetcodeDto.Tags = _mapper.Map<List<StreetcodeTagDTO>>(tagIndexed);

        return Result.Ok(streetcodeDto);
    }
}