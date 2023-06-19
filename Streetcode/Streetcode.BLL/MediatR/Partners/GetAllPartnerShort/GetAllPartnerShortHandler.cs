using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.GetAllPartnerShort
{
    internal class GetAllPartnerShortHandler : IRequestHandler<GetAllPartnersShortQuery, Result<IEnumerable<PartnerShortDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer? _stringLocalizer;

        public GetAllPartnerShortHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<GetAllPartnerShortHandler> stringLocalizer)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<IEnumerable<PartnerShortDTO>>> Handle(GetAllPartnersShortQuery request, CancellationToken cancellationToken)
        {
            var partners = await _repositoryWrapper.PartnersRepository.GetAllAsync();

            if (partners is null)
            {
                return Result.Fail(new Error(_stringLocalizer?["CannotFindPartners"].Value));
            }

            return Result.Ok(_mapper.Map<IEnumerable<PartnerShortDTO>>(partners));
        }
    }
}
