using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl
{
  public class GetStreetcodeByTransliterationUrlHandler : IRequestHandler<GetStreetcodeByTransliterationUrlQuery, Result<StreetcodeDTO>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;

        public GetStreetcodeByTransliterationUrlHandler(IRepositoryWrapper repository, IMapper mapper, IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
        {
            _repository = repository;
            _mapper = mapper;
            _stringLocalizerCannotFind = stringLocalizerCannotFind;
        }

        public async Task<Result<StreetcodeDTO>> Handle(GetStreetcodeByTransliterationUrlQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repository.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    predicate: st => st.TransliterationUrl == request.url);

            if (streetcode == null)
            {
                return new Error(_stringLocalizerCannotFind["CannotFindStreetcodeByTransliterationUrl", request.url].Value);
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
