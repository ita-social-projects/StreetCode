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
            var newPartner = _mapper.Map<Partner>(request.newPartner);

            // add adding phot to blob and getting href for this

            newPartner.Logo = new DAL.Entities.Media.Images.Image()
            {
                Alt = newPartner.Title,
                Title = newPartner.Title,
                Url = "https://t4.ftcdn.net/jpg/00/97/58/97/360_F_97589769_t45CqXyzjz0KXwoBZT9PRaWGHRk5hQqQ.jpg",
            };
            newPartner = (await _repositoryWrapper.PartnersRepository.CreateAsync(newPartner)).Entity;

            try
            {
                if (_repositoryWrapper.SaveChanges() > 0)
                {
                    return Result.Ok(_mapper.Map<PartnerDTO>(newPartner));
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
