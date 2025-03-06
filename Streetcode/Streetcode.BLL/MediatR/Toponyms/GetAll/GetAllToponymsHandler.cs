using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetAll;

public class GetAllToponymsHandler : IRequestHandler<GetAllToponymsQuery,
    Result<GetAllToponymsResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetAllToponymsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<Result<GetAllToponymsResponseDTO>> Handle(GetAllToponymsQuery query, CancellationToken cancellationToken)
    {
        var filterRequest = query.Request;
        Expression<Func<Toponym, bool>>? basePredicate = null;
        var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), query.UserRole, t => t.Streetcodes);

        var toponyms = _repositoryWrapper.ToponymRepository.FindAll(predicate: predicate);

        if (filterRequest.Title is not null)
        {
            FindStreetcodesWithMatchTitle(ref toponyms, filterRequest.Title);
        }

        // int pagesAmount = ApplyPagination(ref toponyms, filterRequest.Amount, filterRequest.Page);

        var toponymDtos = _mapper.Map<IEnumerable<ToponymDTO>>(toponyms.AsEnumerable());

        var response = new GetAllToponymsResponseDTO
        {
            Pages = 1,
            Toponyms = toponymDtos
        };

        return Task.FromResult(Result.Ok(response));
    }

    private void FindStreetcodesWithMatchTitle(
        ref IQueryable<Toponym> toponyms,
        string title)
    {
        toponyms = toponyms.Where(s => s.StreetName
            .ToLower()
            .Contains(title
            .ToLower()))
            .GroupBy(s => s.StreetName)
            .Select(g => g.First());
    }

    // private int ApplyPagination(
    //    ref IQueryable<Toponym> toponyms,
    //    int amount,
    //    int page)
    // {
    //    var totalPages = (int)Math.Ceiling(toponyms.Count() / (double)amount);

    // toponyms = toponyms
    //        .Skip((page - 1) * amount)
    //        .Take(amount);

    // return totalPages;
    // }
}