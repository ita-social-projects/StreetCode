using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.BLL.MediatR.Jobs.ChangeStatus
{
	public record ChangeJobStatusCommand(JobChangeStatusDto jobChangeStatusDto)
		: IRequest<Result<int>>;
}
