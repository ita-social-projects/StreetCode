using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Util;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;


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

            partnerDb.Title = partner.Title;
            partnerDb.Description = partner.Description;
            partnerDb.IsKeyPartner = partner.IsKeyPartner;
            partnerDb.TargetUrl = partner.TargetUrl;

            var partnerOldStreetcodesId = partnerDb.Streetcodes.Select(s => s.Id);

            var deletedLinks = partnerDb.PartnerSourceLinks.Except(partner.PartnerSourceLinks, new IdComparer()).ToList();
            deletedLinks.ForEach(link => _repositoryWrapper.PartnerSourceLinkRepository.Delete(link));

            var newPartnerStreetcodeIds = request.Partner.Streetcodes.Select(s => s.Id).ToList();
            var idsToDelete = partnerOldStreetcodesId.Except(newPartnerStreetcodeIds);
            var idsToAdd = newPartnerStreetcodeIds.Except(partnerOldStreetcodesId);
            string ids = "";
            string idsAdd = "";
            foreach (int id in idsToDelete)
            {
                ids += $"{id}, ";
            }

            foreach (int id in idsToAdd)
            {
                idsAdd += $"({partner.Id}, {id}),";
            }

            string sqlcommand = "";
            if (ids.Length > 2)
            {
                sqlcommand += $"DELETE FROM [streetcode].[streetcode_partners] WHERE PartnersId = {partner.Id} and StreetcodesId IN ({ids.Substring(0, ids.Length - 2)});";
            } 
            if(idsAdd.Length > 2)
            {
                sqlcommand += $"INSERT INTO [streetcode].[streetcode_partners] (PartnersId, StreetcodesId) VALUES {idsAdd.Substring(0, idsAdd.Length - 1)};";
            }
            
            // update logo if base64 is not empty
            try
            {
                partnerDb.Streetcodes.Clear();
                partnerDb = _repositoryWrapper.PartnersRepository.Update(partnerDb).Entity;
                _repositoryWrapper.SaveChanges();
                if (sqlcommand.Length > 0)
                {
                    _repositoryWrapper.PartnersRepository.ExecuteSQL(sqlcommand);
                }

                var mappedPartner = _mapper.Map<PartnerDTO>(partnerDb);
                mappedPartner.Streetcodes = request.Partner.Streetcodes;
                return Result.Ok(_mapper.Map<PartnerDTO>(mappedPartner));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
