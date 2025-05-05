using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Team.GetAll;

public class GetAllTeamHandler : IRequestHandler<GetAllTeamQuery, Result<GetAllTeamDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetAllTeamHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<GetAllTeamDTO>> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
    {
        var searchTitle = request.title?.Trim().ToLower();
        int page = request.page ?? 1;
        int pageSize = request.pageSize ?? 10;

        var allTeams = await _repositoryWrapper
            .TeamRepository
            .GetAllAsync(
                predicate: x => true,
                include: x => x
                    .Include(x => x.Positions)
                    .Include(x => x.TeamMemberLinks)
                    .Include(x => x.Image));

        var filteredContexts = allTeams.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTitle))
        {
            filteredContexts = filteredContexts.Where(context =>
                (!string.IsNullOrWhiteSpace(context.Name) &&
                 context.Name.ToLower().Contains(searchTitle)) ||
                (context.Positions != null &&
                 context.Positions.Any(pos =>
                     !string.IsNullOrWhiteSpace(pos.Position) &&
                     pos.Position.ToLower().Contains(searchTitle))));
        }

        if (request.IsMain.HasValue)
        {
            filteredContexts = filteredContexts.Where(context => context.IsMain == request.IsMain.Value);
            page = 1;
        }

        var totalItems = filteredContexts.Count();

        var paginatedContexts = filteredContexts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var teamMemberDtos = MapToTeamMemberDtos(paginatedContexts);

        var getAllTeamDto = new GetAllTeamDTO()
        {
            TotalAmount = totalItems,
            TeamMembers = teamMemberDtos,
        };

        return Result.Ok(getAllTeamDto);
    }

    private IEnumerable<TeamMemberDTO> MapToTeamMemberDtos(IEnumerable<TeamMember> teamMemberEntities)
    {
        var teamMemberDtosList = _mapper.Map<IEnumerable<TeamMemberDTO>>(teamMemberEntities).ToList();

        foreach (var teamMemberDto in teamMemberDtosList)
        {
            if (teamMemberDto.Image is not null)
            {
                teamMemberDto.Image.Base64 = _blobService.FindFileInStorageAsBase64(teamMemberDto.Image.BlobName);
            }
        }

        return teamMemberDtosList;
    }
}
