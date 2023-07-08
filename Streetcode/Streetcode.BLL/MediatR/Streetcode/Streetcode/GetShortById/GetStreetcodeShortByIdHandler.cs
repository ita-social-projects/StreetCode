using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById
{
    public class GetStreetcodeShortByIdHandler : IRequestHandler<GetStreetcodeShortByIdQuery, Result<StreetcodeShortDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
        private readonly IStringLocalizer<CannotMapSharedResource> _stringLocalizerCannotMap;

        public GetStreetcodeShortByIdHandler(
            IMapper mapper,
            IRepositoryWrapper repository,
            IStringLocalizer<CannotMapSharedResource> stringLocalizerCannotMap,
            IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _mapper = mapper;
            _repository = repository;
            _stringLocalizerCannotMap = stringLocalizerCannotMap;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<StreetcodeShortDTO>> Handle(GetStreetcodeShortByIdQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repository.StreetcodeRepository.GetFirstOrDefaultAsync(st => st.Id == request.id);

            if (streetcode == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotFind["CannotFindStreetcodeById"].Value));
            }

            var streetcodeShortDTO = _mapper.Map<StreetcodeShortDTO>(streetcode);

            if(streetcodeShortDTO == null)
            {
                return Result.Fail(new Error(_stringLocalizerCannotMap["CannotMapStreetcodeToShortDTO"].Value));
            }

            return Result.Ok(streetcodeShortDTO);
        }
    }
}
