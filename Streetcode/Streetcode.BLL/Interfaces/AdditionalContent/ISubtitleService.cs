using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.Interfaces.AdditionalContent;

public interface ISubtitleService
{
    public Task<IEnumerable<SubtitleDTO>> GetSubtitlesByStreetcodeAsync();
}