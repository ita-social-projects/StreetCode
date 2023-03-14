using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
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
            updated = _repositoryWrapper.PartnersRepository.Update(_mapper.Map<Partner>(request.Partner)).Entity;
            
            //update logo if base64 is not empty

            if (updated != null)
            {
                try
                {
                    _repositoryWrapper.SaveChanges();

                    return Result.Ok(_mapper.Map<PartnerDTO>(updated));
                }
                catch(Exception ex)
                {
                    return Result.Fail(ex.Message);
                }
            }

            return Result.Fail("Cannot update it");
        }
    }
}
