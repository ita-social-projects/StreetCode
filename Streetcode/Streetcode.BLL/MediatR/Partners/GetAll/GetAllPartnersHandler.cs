using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Entities.Partners;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Partners.GetAll;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, Result<GetAllPartnersResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;

    public GetAllPartnersHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizeCannotFind = stringLocalizeCannotFind;
    }

    public Task<Result<GetAllPartnersResponseDTO>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        var allPartnersQuery = _repositoryWrapper
            .PartnersRepository
            .FindAll(include: partnersCollection => partnersCollection
                .Include(pl => pl.PartnerSourceLinks)
                .Include(p => p.Streetcodes));

        if (!await allPartnersQuery.AnyAsync(cancellationToken))
        {
            string errorMsg = _stringLocalizeCannotFind["CannotFindAnyPartners"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<GetAllPartnersResponseDTO>(new Error(errorMsg)));
        }

        if (!string.IsNullOrWhiteSpace(request.title))
        {
            allPartnersQuery = allPartnersQuery.Where(p => p.Title.ToLower().Contains(request.title.ToLower()));
        }

        if (request.IsKeyPartner.HasValue)
        {
            allPartnersQuery = allPartnersQuery.Where(p => p.IsKeyPartner == request.IsKeyPartner.Value);
        }

        var totalCount = await allPartnersQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            var emptyResponse = new GetAllPartnersResponseDTO
            {
                TotalAmount = 0,
                Partners = new List<PartnerDTO>()
            };

            return Result.Ok(emptyResponse);
        }

        var page = request.page ?? 1;
        var pageSize = request.pageSize ?? 10;

        var paginatedPartners = await allPartnersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var getAllPartnersResponseDTO = new GetAllPartnersResponseDTO()
        {
            TotalAmount = totalCount,
            Partners = _mapper.Map<IEnumerable<PartnerDTO>>(paginatedPartners),
        };

        return Task.FromResult(Result.Ok(getAllPartnersResponseDTO));
    }
}
