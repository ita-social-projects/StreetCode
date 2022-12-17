using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.AdditionalContent;

public class SubtitleService : ISubtitleService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    public SubtitleService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubtitleDTO>> GetSubtitlesByStreetcodeAsync()
    {
        var subtitle = await _repositoryWrapper.SubtitleRepository.GetAllAsync(c => c.StreetcodeId == 1);
        return _mapper.Map<IEnumerable<Subtitle>, IEnumerable<SubtitleDTO>>(subtitle);

        // TODO clean after merge
    }
}