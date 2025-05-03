using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Jobs.GetAllShort
{
	public record GetAllShortJobsQuery(UserRole? UserRole)
		: IRequest<Result<IEnumerable<JobShortDto>>>;
}
