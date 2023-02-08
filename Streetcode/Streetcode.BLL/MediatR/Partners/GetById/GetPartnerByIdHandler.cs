using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetById;

public class GetPartnerByIdHandler : IRequestHandler<GetPartnerByIdQuery, Result<PartnerDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetPartnerByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<PartnerDTO>> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
    {
        var partner = await _repositoryWrapper
            .PartnersRepository
            .GetSingleOrDefaultAsync(
                predicate: p => p.Id == request.Id,
                include: p => p
                    .Include(pl => pl.PartnerSourceLinks));

        if (partner is null)
        {
            return Result.Fail(new Error($"Cannot find any partner with corresponding id: {request.Id}"));
        }

        var partnerDto = _mapper.Map<PartnerDTO>(partner);
        return Result.Ok(partnerDto);
    }
}