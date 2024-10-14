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

    public GetAllPartnersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizeCannotFind = stringLocalizeCannotFind;
    }

    public Task<Result<GetAllPartnersResponseDTO>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        PaginationResponse<Partner> paginationResponse = _repositoryWrapper
                .PartnersRepository
                .GetAllPaginated(
                    request.page,
                    request.pageSize,
                    include: partnersCollection => partnersCollection
                        .Include(pl => pl.PartnerSourceLinks)
                        .Include(p => p.Streetcodes));

        if (paginationResponse is null)
        {
            string errorMsg = _stringLocalizeCannotFind["CannotFindAnyPartners"].Value;
            _logger.LogError(request, errorMsg);
            return Task.FromResult(Result.Fail<GetAllPartnersResponseDTO>(new Error(errorMsg)));
        }

        GetAllPartnersResponseDTO getAllPartnersResponseDTO = new GetAllPartnersResponseDTO()
        {
            TotalAmount = paginationResponse.TotalItems,
            Partners = _mapper.Map<IEnumerable<PartnerDTO>>(paginationResponse.Entities),
        };

        return Task.FromResult(Result.Ok(getAllPartnersResponseDTO));
    }
}
