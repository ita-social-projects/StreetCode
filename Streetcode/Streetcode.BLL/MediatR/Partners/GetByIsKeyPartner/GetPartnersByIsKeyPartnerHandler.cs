using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetByIsKeyPartner;

public class GetPartnersByIsKeyPartnerHandler : IRequestHandler<GetPartnersByIsKeyPartnerQuery, Result<IEnumerable<PartnerDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizeCannotFind;

    public GetPartnersByIsKeyPartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IStringLocalizer<CannotFindSharedResource> stringLocalizeCannotFind)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizeCannotFind = stringLocalizeCannotFind;
    }

    public async Task<Result<IEnumerable<PartnerDTO>>> Handle(GetPartnersByIsKeyPartnerQuery request, CancellationToken cancellationToken)
    {
        var partners = await _repositoryWrapper
            .PartnersRepository
            .GetAllAsync(
                predicate: p => p.IsKeyPartner == request.IsKeyPartner,
                include: p => p
                    .Include(pl => pl.PartnerSourceLinks)
                    .Include(p => p.Streetcodes));

        if (partners is null)
        {
            string errorMessage = _stringLocalizeCannotFind["CannotFindAnyPartners"].Value;
            _logger.LogError(request, errorMessage);
            return Result.Fail(new Error(errorMessage));
        }

        return Result.Ok(_mapper.Map<IEnumerable<PartnerDTO>>(partners));
    }
}