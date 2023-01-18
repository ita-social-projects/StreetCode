using FluentResults;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.Interfaces.Streetcode;

public interface IRelatedFigureService
{
    public void GetRelatedFiguresByStreetcodeId(int streetcodeId);
}
