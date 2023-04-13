using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Partners.Create
{
    public class CreatePartnerHandler : IRequestHandler<CreatePartnerQuery, Result<PartnerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CreatePartnerHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<PartnerDTO>> Handle(CreatePartnerQuery request, CancellationToken cancellationToken)
        {
            var newPartner = _mapper.Map<Partner>(request.newPartner);
            newPartner.Logo = new Image();
            try
            {
                newPartner.Streetcodes.Clear();
                newPartner = await _repositoryWrapper.PartnersRepository.CreateAsync(newPartner);
                _repositoryWrapper.SaveChanges();
                var streetcodeIds = request.newPartner.Streetcodes.Select(s => s.Id).ToList();
                newPartner.Streetcodes.AddRange(await _repositoryWrapper
                    .StreetcodeRepository
                    .GetAllAsync(s => streetcodeIds.Contains(s.Id)));

                _repositoryWrapper.SaveChanges();
                return Result.Ok(_mapper.Map<PartnerDTO>(newPartner));
            }
            catch(Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
