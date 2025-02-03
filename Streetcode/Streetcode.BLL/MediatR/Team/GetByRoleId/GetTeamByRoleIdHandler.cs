using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetByRoleId
{
    public class GetTeamByRoleIdHandler : IRequestHandler<GetTeamByRoleIdQuery, Result<IEnumerable<TeamMemberDTO>>>
	{
		private readonly IMapper _mapper;
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly ILoggerService _logger;
		private readonly IBlobService _blob;

		public GetTeamByRoleIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger, IBlobService blob)
		{
			_repositoryWrapper = repositoryWrapper;
			_mapper = mapper;
			_logger = logger;
			_blob = blob;
		}

		public async Task<Result<IEnumerable<TeamMemberDTO>>> Handle(GetTeamByRoleIdQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var teamDtoByRoleId = await _repositoryWrapper.TeamRepository.GetAllAsync(
					predicate: t => t.Positions!.Any(p => p.Id == request.roleId),
					include: t =>
						t.Include(tm => tm.TeamMemberLinks).Include(tm => tm.Image!));
				teamDtoByRoleId = teamDtoByRoleId
					.Select(team =>
					{
						if (team.Image != null)
						{
							team.Image.Base64 = _blob.FindFileInStorageAsBase64(team.Image.BlobName!);
						}

						return team;
					})
				.ToList();
				var teamByRoleId = _mapper.Map<IEnumerable<TeamMemberDTO>>(teamDtoByRoleId);

				return Result.Ok(teamByRoleId);
			}
			catch (Exception ex)
			{
                _logger.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
			}
		}
	}
}
