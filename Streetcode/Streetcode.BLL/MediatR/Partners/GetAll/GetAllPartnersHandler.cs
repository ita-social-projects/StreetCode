using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<GetAllPartnersDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetAllPartnersHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public Task<Result<GetAllPartnersDto>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        var paginationResponse = _repositoryWrapper
                .PartnersRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    include: partnersCollection => partnersCollection
                        .Include(x => x.PartnerSourceLinks)
                        .Include(x => x.Streetcodes)
                        .Include(x => x.Logo!));

        var partnersDtos = MapToPartnerDtos(paginationResponse.Entities);
        var getAllPartnersDto = new GetAllPartnersDto()
        {
            TotalAmount = paginationResponse.TotalItems,
            Partners = partnersDtos,
        };

        return Task.FromResult(Result.Ok(getAllPartnersDto));
    }

    private IEnumerable<PartnerDto> MapToPartnerDtos(IEnumerable<Partner> partnerEntities)
    {
        var partnerDtosList = _mapper.Map<IEnumerable<PartnerDto>>(partnerEntities).ToList();

        foreach (var partnerDto in partnerDtosList)
        {
            if (partnerDto.Logo is not null)
            {
                partnerDto.Logo.Base64 = _blobService.FindFileInStorageAsBase64(partnerDto.Logo.BlobName);
            }
        }

        return partnerDtosList;
    }
}
