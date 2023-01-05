using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<IEnumerable<PartnerDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllPartnersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        var partner = await _repositoryWrapper.PartnersRepository.GetAllAsync();

        var partnerDtos = _mapper.Map<IEnumerable<PartnerDTO>>(partner);
        return Result.Ok(partnerDtos);
    }
}
