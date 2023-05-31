using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update
{
	public record class UpdateStreetcodeCommand(StreetcodeUpdateDTO Streetcode) : IRequest<Result<StreetcodeUpdateDTO>>;
}
