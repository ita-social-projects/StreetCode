using System.Text;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetNamePosition;
using Streetcode.DAL.Repositories.Interfaces.Base;

public class GetTickerStringHandler : IRequestHandler<GetTickerStringQuery, Result<string>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly ILoggerService _loggerService;

    public GetTickerStringHandler(IRepositoryWrapper repository, ILoggerService loggerService)
    {
        _repository = repository;
        _loggerService = loggerService;
    }

    public async Task<Result<string>> Handle(GetTickerStringQuery request, CancellationToken cancellationToken)
    {
        var positions = await _repository.PositionRepository.GetAllAsync(include: p => p.Include(src => src.TeamMembers!));
        string a = string.Empty;
        StringBuilder memberString = new StringBuilder();

        foreach (var position in positions)
        {
            if (position.TeamMembers is not null && position.TeamMembers.Count > 0)
            {
                memberString.Append(position.Position);
                memberString.Append(": ");

                int memberCount = position.TeamMembers.Count;
                for (int i = 0; i < memberCount; i++)
                {
                    memberString.Append(position.TeamMembers[i].Name);

                    if (i < memberCount - 1)
                    {
                        memberString.Append(", ");
                    }
                }

                memberString.Append(" ");
            }
        }

        try
        {
            return Result.Ok(memberString.ToString());
        }
        catch (Exception ex)
        {
            _loggerService.LogError(request, ex.Message);
            return Result.Fail<string>(ex.Message);
        }
    }
}
