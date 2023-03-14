using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Partners;
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
            var newItem = (await _repositoryWrapper.PartnersRepository.CreateAsync(_mapper.Map<Partner>(request.newPartner))).Entity;
            try
            {
                if (_repositoryWrapper.SaveChanges() > 0)
                {
                    return Result.Ok(_mapper.Map<PartnerDTO>(newItem));
                }
            }
            catch(Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            return Result.Fail("The partner wasn`t added");
        }
    }
}
