using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
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
					predicate: t => t.Positions!.Where(p => p.Id == request.roleId).Count() > 0,
					include: t =>
						t.Include(tm => tm.TeamMemberLinks).Include(tm => tm.Image!));

				foreach (var team in teamDtoByRoleId)
				{
					team.Image!.Base64 = _blob.FindFileInStorageAsBase64(team.Image.BlobName!);
				}

				var teamByRoleId = _mapper.Map<IEnumerable<TeamMemberDTO>>(teamDtoByRoleId);

				return Result.Ok(teamByRoleId);
			}
			catch (Exception ex)
			{
				return Result.Fail(ex.Message);
			}
		}
	}
}
