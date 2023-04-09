using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl
{
    public class GetStreetcodeByTransliterationUrlHandler : IRequestHandler<GetStreetcodeByTransliterationUrlQuery, Result<StreetcodeDTO>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetStreetcodeByTransliterationUrlHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<StreetcodeDTO>> Handle(GetStreetcodeByTransliterationUrlQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repository.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    predicate: st => st.TransliterationUrl == request.url);

            if (streetcode == null)
            {
                return new Error($"Cannot find streetcode by transliteration url: {request.url}");
            }

            var tagIndexed = await _repository.StreetcodeTagIndexRepository
                                            .GetAllAsync(
                                                t => t.StreetcodeId == streetcode.Id,
                                                include: q => q.Include(ti => ti.Tag));

            var streetcodeDTO = _mapper.Map<StreetcodeDTO>(streetcode);
            streetcodeDTO.Tags = _mapper.Map<List<StreetcodeTagDTO>>(tagIndexed);
            return Result.Ok(streetcodeDTO);
        }
    }
}
