using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;

public class GetStreetcodeByIdHandler : IRequestHandler<GetStreetcodeByIdQuery, Result<StreetcodeDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetStreetcodeByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<StreetcodeDTO>> Handle(GetStreetcodeByIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
            predicate: st => st.Id == request.Id);

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot find any streetcode with corresponding id: {request.Id}"));
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