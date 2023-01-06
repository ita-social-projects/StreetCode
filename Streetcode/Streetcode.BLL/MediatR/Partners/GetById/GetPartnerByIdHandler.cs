using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partner.GetById;

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
        var partner = await _repositoryWrapper.PartnersRepository
            .GetFirstOrDefaultAsync(st => st.Id == request.id);

        if (partner is null)
        {
            return Result.Fail(new Error($"Cannot find a Partner with corresponding Id: {request.id}"));
        }

        var partnerDto = _mapper.Map<PartnerDTO>(partner);
        return Result.Ok(partnerDto);
    }
}