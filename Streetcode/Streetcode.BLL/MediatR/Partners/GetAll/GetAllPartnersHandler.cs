using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<GetAllPartnersDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;
    private readonly IBlobService _blobService;

    public GetAllPartnersHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind,
        IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizeCannotFind = stringLocalizeCannotFind;
        _blobService = blobService;
    }

    public async Task<Result<GetAllPartnersDto>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        var searchTitle = request.title?.Trim().ToLower();
        int page = request.page ?? 1;
        int pageSize = request.pageSize ?? 10;
        var partners = await _repositoryWrapper.PartnersRepository
            .GetAllAsync(include: partnersCollection => partnersCollection
                .Include(pl => pl.PartnerSourceLinks)
                .Include(p => p.Streetcodes)
                .Include(p => p.Logo));

        if (!string.IsNullOrWhiteSpace(searchTitle))
        {
            partners = partners.Where(context =>
                !string.IsNullOrWhiteSpace(context.Title) &&
                context.Title.ToLower().Contains(searchTitle));
        }

        if (request.IsKeyPartner.HasValue)
        {
            partners = partners.Where(context =>
                context.IsKeyPartner == request.IsKeyPartner.Value);
            page = 1;
        }

        var totalAmount = partners.Count();
        var paginatedPartners = partners
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var partnersDtos = MapToPartnerDtos(paginatedPartners);

        var getAllPartnersResponseDTO = new GetAllPartnersDto
        {
            TotalAmount = totalAmount,
            Partners = partnersDtos,
        };

        return Result.Ok(getAllPartnersResponseDTO);
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