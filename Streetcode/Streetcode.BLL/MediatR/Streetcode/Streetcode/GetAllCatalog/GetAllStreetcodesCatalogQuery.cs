using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog
{
    public record GetAllStreetcodesCatalogQuery(int page, int count) : IRequest<Result<IEnumerable<RelatedFigureDTO>>>;
}
