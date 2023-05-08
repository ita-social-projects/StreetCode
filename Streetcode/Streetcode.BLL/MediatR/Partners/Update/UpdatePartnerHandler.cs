using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Util;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Update
{
    public class UpdatePartnerHandler : IRequestHandler<UpdatePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UpdatePartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<PartnerDTO>> Handle(UpdatePartnerQuery request, CancellationToken cancellationToken)
        {
            var partner = _mapper.Map<Partner>(request.Partner);

            try
            {
                var links = await _repositoryWrapper.PartnerSourceLinkRepository
                   .GetAllAsync(predicate: l => l.PartnerId == partner.Id);

                var newLinkIds = partner.PartnerSourceLinks.Select(l => l.Id).ToList();

                foreach (var link in links)
                {
                    if (!newLinkIds.Contains(link.Id))
                    {
                        _repositoryWrapper.PartnerSourceLinkRepository.Delete(link);
                    }
                }

                partner.Streetcodes.Clear();
                _repositoryWrapper.PartnersRepository.Update(partner);
                _repositoryWrapper.SaveChanges();
                var newStreetcodeIds = request.Partner.Streetcodes.Select(s => s.Id).ToList();
                var oldStreetcodes = await _repositoryWrapper.PartnerStreetcodeRepository
                    .GetAllAsync(ps => ps.PartnerId == partner.Id);

                foreach (var old in oldStreetcodes!)
                {
                    if (!newStreetcodeIds.Contains(old.StreetcodeId))
                    {
                        _repositoryWrapper.PartnerStreetcodeRepository.Delete(old);
                    }
                }

                foreach (var newCodeId in newStreetcodeIds!)
                {
                    if (oldStreetcodes.FirstOrDefault(x => x.StreetcodeId == newCodeId) == null)
                    {
                        _repositoryWrapper.PartnerStreetcodeRepository.Create(
                            new StreetcodePartner() { PartnerId = partner.Id, StreetcodeId = newCodeId });
                    }
                }

                _repositoryWrapper.SaveChanges();
                var dbo = _mapper.Map<PartnerDTO>(partner);
                dbo.Streetcodes = request.Partner.Streetcodes;
                return Result.Ok(dbo);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
