using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Jobs.GetAll
{
	public record GetAllJobsQuery(UserRole? UserRole, ushort? page, ushort? pageSize, string? title = null)
		: IRequest<Result<GetAllJobsDTO>>;
}
