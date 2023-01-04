using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId
{
    public class GetAudioByStreetcodeIdQueryHandler : IRequestHandler<GetAudioByStreetcodeIdQuery, Result<IEnumerable<AudioDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAudioByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AudioDTO>>> Handle(GetAudioByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var audio = await _repositoryWrapper.AudioRepository.GetAllAsync(audio => audio.StreetcodeId == request.streetcodeId);
            if (audio == null)
            {
                return Result.Fail(new Error("Can`t find audio with this StreetcodeId"));
            }

            var audioDto = _mapper.Map<IEnumerable<AudioDTO>>(audio);
            return Result.Ok(audioDto);
        }
    }
}
