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

    public Task<Result<GetAllTeamDTO>> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
    {
        var paginationResponse = _repositoryWrapper
            .TeamRepository
            .GetAllPaginated(
                request.page,
                request.pageSize,
                include: x => x
                    .Include(x => x.Positions)
                    .Include(x => x.TeamMemberLinks!)
                    .Include(x => x.Image!));

        var teamMemberDtos = MapToTeamMemberDtos(paginationResponse.Entities);
        var getAllTeamDto = new GetAllTeamDTO()
        {
            TotalAmount = paginationResponse.TotalItems,
            TeamMembers = teamMemberDtos,
        };

        return Task.FromResult(Result.Ok(getAllTeamDto));
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
