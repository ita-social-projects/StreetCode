using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

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

    public void GetSubtitlesByStreetcodeAsync()
    {
        // TODO implement here
    }
}