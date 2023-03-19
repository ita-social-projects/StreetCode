using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Partners;
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
            var partner = await _repositoryWrapper.PartnersRepository
                .GetFirstOrDefaultAsync(
                    p => p.Id == request.Partner.Id,
                    include: s => s.Include(s => s.Streetcodes)
                        .Include(s => s.PartnerSourceLinks));
            if (partner == null)
            {
                return Result.Fail("No such partner");
            }

            var newPartnerStreetcodeIds = request.Partner.Streetcodes.Select(s => s.Id).ToList();

            var newPartnerStreetcodes = await _repositoryWrapper
               .StreetcodeRepository
               .GetAllAsync(
                s => newPartnerStreetcodeIds
                    .Contains(s.Id), include: s => s.Include(s => s.Partners));

            var newPartnerLinkIds = request.Partner.PartnerSourceLinks.Select(s => s.Id).ToList();
            var newPartnerLinks = await _repositoryWrapper
                .PartnerSourceLinkRepository
                .GetAllAsync(l => newPartnerLinkIds.Contains(l.Id));

            partner.Streetcodes.Clear();
            partner.Streetcodes.AddRange(newPartnerStreetcodes);

            partner.IsKeyPartner = request.Partner.IsKeyPartner;
            partner.TargetUrl = request.Partner.TargetUrl;
            partner.Description = request.Partner.Description;
            partner.Title = request.Partner.Title;
            partner.UrlTitle = request.Partner.UrlTitle;

            var newLinks = _mapper.Map<IEnumerable<PartnerSourceLink>>(
                    request.Partner.PartnerSourceLinks.Where(p => p.Id == 0)).ToList();
            newLinks.ForEach(l =>
            {
                l.Partner = partner;
                l.PartnerId = partner.Id;
            });
            partner.PartnerSourceLinks = newPartnerLinks.ToList();
            partner.PartnerSourceLinks.AddRange(newLinks);

            // update logo if base64 is not empty
            try
            {
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
