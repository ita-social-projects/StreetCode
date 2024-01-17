using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Jobs.Delete
{
	public record DeleteJobCommand(int id)
		: IRequest<Result<int>>;
}