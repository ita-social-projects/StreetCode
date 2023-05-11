using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById
{
    public class GetStreetcodeShortByIdHandler : IRequestHandler<GetStreetcodeShortByIdQuery, Result<StreetcodeShortDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public GetStreetcodeShortByIdHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<StreetcodeShortDTO>> Handle(GetStreetcodeShortByIdQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(st => st.Id == request.id);

            if (streetcode == null)
            {
                return Result.Fail(new Error("Cannot find streetcode by id"));
            }

            var streetcodeShortDTO = _mapper.Map<StreetcodeShortDTO>(streetcode);

            if(streetcodeShortDTO == null)
            {
                return Result.Fail(new Error("Cannot map streetcode to shortDTO"));
            }

            return Result.Ok(streetcodeShortDTO);
        }
    }
}
