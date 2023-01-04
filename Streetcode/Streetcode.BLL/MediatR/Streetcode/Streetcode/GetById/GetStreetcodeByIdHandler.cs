using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            predicate: st => st.Id == request.id,
            include: source => source.Include(l => l.Tags));

        if (streetcode is null)
        {
            return Result.Fail(new Error($"Cannot find a fact with corresponding Id: {request.id}"));
        }

        var streetcodeDto = _mapper.Map<StreetcodeDTO>(streetcode);
        return Result.Ok(streetcodeDto);
    }
}