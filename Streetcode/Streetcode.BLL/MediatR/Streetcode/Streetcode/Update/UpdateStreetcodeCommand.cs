using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.Update;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update
{
	public record class UpdateStreetcodeCommand(StreetcodeUpdateDTO Streetcode)
		: IRequest<Result<int>>;
}
