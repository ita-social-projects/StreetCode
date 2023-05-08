using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Filter;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByFilter
{
    public record class GetStreetcodeByFilterQuery(StreetcodeFilterRequestDTO Filter) : IRequest<Result<List<StreetcodeFilterResultDTO>>>;
}
