using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Streetcode.Types;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Queries
{
    public record GetPersonStreetcodeByIdQuery(int id) : IRequest<Result<PersonStreetcodeDTO>>;
}
