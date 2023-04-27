using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Util;
using Streetcode.DAL.Entities.Partners;
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
            var partnerDb = await _repositoryWrapper.PartnersRepository
                .GetFirstOrDefaultAsync(
                    p => p.Id == partner.Id,
                    include: q => q.Include(p => p.PartnerSourceLinks).Include(p => p.Streetcodes));

            if (partnerDb == null || partner == null)
            {
                return Result.Fail("No such partner");
            }

            var partnerOldStreetcodesId = partnerDb.Streetcodes.Select(s => s.Id);

            var deletedLinks = partnerDb.PartnerSourceLinks.Except(partner.PartnerSourceLinks, new IdComparer()).ToList();
            deletedLinks.ForEach(link => _repositoryWrapper.PartnerSourceLinkRepository.Delete(link));

            var newPartnerStreetcodeIds = request.Partner.Streetcodes.Select(s => s.Id).ToList();
            var idsToDelete = partnerOldStreetcodesId.Except(newPartnerStreetcodeIds);
            var idsToAdd = newPartnerStreetcodeIds.Except(partnerOldStreetcodesId);

            foreach(var id in idsToDelete)
            {
                _repositoryWrapper.PartnerStreetcodeRepository.Delete(new StreetcodePartner()
                {
                    PartnerId = partner.Id,
                    StreetcodeId = id,
                });
            }

            foreach (var id in idsToAdd)
            {
                await _repositoryWrapper.PartnerStreetcodeRepository.CreateAsync(new StreetcodePartner()
                {
                    PartnerId = partner.Id,
                    StreetcodeId = id,
                });
            }

            try
            {
                _repositoryWrapper.SaveChanges();
                partner.Streetcodes.Clear();
                _repositoryWrapper.PartnersRepository.Update(partner);
                _repositoryWrapper.SaveChanges();
                return Result.Ok(_mapper.Map<PartnerDTO>(partner));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
