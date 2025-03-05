using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog;

public record GetAllStreetcodesCatalogQuery(int Page, int Count)
    : IRequest<Result<IEnumerable<CatalogItem>>>;