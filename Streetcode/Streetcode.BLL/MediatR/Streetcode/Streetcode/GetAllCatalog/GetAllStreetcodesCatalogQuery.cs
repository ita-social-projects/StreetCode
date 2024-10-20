using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog
{
    public record GetAllStreetcodesCatalogQuery(int page, int count)
        : IRequest<Result<IEnumerable<CatalogItem>>>;
}
