using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Users; // Додаємо необхідний using для доступу до User

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public class GetAllStreetcodesHandler : IRequestHandler<GetAllStreetcodesQuery, Result<GetAllStreetcodesResponseDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
    private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizerFailedToValidate;

    public GetAllStreetcodesHandler(
        IRepositoryWrapper repositoryWrapper,
        IMapper mapper,
        ILoggerService logger,
        IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind,
        IStringLocalizer<FailedToValidateSharedResource> stringLocalizerFailedToValidate)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _stringLocalizerCannotFind = stringLocalizerCannotFind;
        _stringLocalizerFailedToValidate = stringLocalizerFailedToValidate;
    }

    public Task<Result<GetAllStreetcodesResponseDTO>> Handle(GetAllStreetcodesQuery query, CancellationToken cancellationToken)
    {
        var filterRequest = query.Request;

        var users = _repositoryWrapper.UserRepository
            .FindAll()
            .AsQueryable();

        var streetcodes = _repositoryWrapper.StreetcodeRepository
            .FindAll()
            .Include(s => s.Tags)
            .AsQueryable();

        if (filterRequest.Title is not null)
        {
            FindStreetcodesWithMatchTitle(ref streetcodes, filterRequest.Title, users);
        }

        if (filterRequest.Sort is not null)
        {
            var sortedResult = FindSortedStreetcodes(ref streetcodes, filterRequest.Sort);
            if (sortedResult.IsFailed)
            {
                var errorMsg = _stringLocalizerCannotFind[sortedResult.Errors[0].Message].Value;
                _logger.LogError(query, errorMsg);
                return Task.FromResult<Result<GetAllStreetcodesResponseDTO>>(Result.Fail(new Error(errorMsg)));
            }
        }

        if (filterRequest.Filter is not null)
        {
            var filterResult = FindFilteredStreetcodes(ref streetcodes, filterRequest.Filter);
            if (filterResult.IsFailed)
            {
                var errorMsg = _stringLocalizerCannotFind[filterResult.Errors[0].Message].Value;
                _logger.LogError(query, errorMsg);
                return Task.FromResult<Result<GetAllStreetcodesResponseDTO>>(Result.Fail(new Error(errorMsg)));
            }
        }

        var totalAmount = streetcodes.Count();

        if (filterRequest.Amount is not null && filterRequest.Page is not null)
        {
            if (filterRequest.Page <= 0 || filterRequest.Amount <= 0)
            {
                var errorMsg = _stringLocalizerFailedToValidate["InvalidPaginationParameters"].Value;
                _logger.LogError(query, errorMsg);
                return Task.FromResult<Result<GetAllStreetcodesResponseDTO>>(Result.Fail(new Error(errorMsg)));
            }

            ApplyPagination(ref streetcodes, filterRequest.Amount!.Value, filterRequest.Page!.Value);
        }

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(streetcodes.AsEnumerable());

        var response = new GetAllStreetcodesResponseDTO
        {
            TotalAmount = totalAmount,
            Streetcodes = streetcodeDtos
        };

        return Task.FromResult(Result.Ok(response));
    }

    private static void FindStreetcodesWithMatchTitle(
    ref IQueryable<StreetcodeContent> streetcodes,
    string title,
    IQueryable<User> users) 
    {
        streetcodes = streetcodes
        .Join(users, streetcode => streetcode.UserId, user => user.Id, (streetcode, user) => new { streetcode, user }) 
        .Where(sc => sc.streetcode.Title!.ToLower().Contains(title.ToLower())
             || sc.streetcode.Index.ToString() == title
             || (sc.user != null && sc.user.UserName.ToLower().Contains(title.ToLower()))) 
        .Select(sc => sc.streetcode); 

        var resultList = streetcodes.ToList();

        foreach (var streetcode in resultList)
        {
            var user = users.FirstOrDefault(u => u.Id == streetcode.UserId); 
            Console.WriteLine($"Title: {streetcode.Title}, Author (User Name): {user?.UserName ?? "Unknown"}");
        }
    }

    private static Result FindFilteredStreetcodes(
        ref IQueryable<StreetcodeContent> streetcodes,
        string filter)
    {
        var filterParams = filter.Split(':');
        var filterColumn = filterParams[0];
        var filterValue = filterParams[1];

        var type = typeof(StreetcodeContent);
        var property = type.GetProperty(filterColumn);

        if (property is null)
        {
            return Result.Fail(new Error("CannotFindAnyPropertyWithThisName"));
        }

        streetcodes = streetcodes
            .AsEnumerable()
            .Where(s => property.GetValue(s)?.ToString()?.Contains(filterValue, StringComparison.OrdinalIgnoreCase) ?? false)
            .AsQueryable();

        return Result.Ok();
    }

    private static Result FindSortedStreetcodes(
        ref IQueryable<StreetcodeContent> streetcodes,
        string sort)
    {
        var sortedRecords = streetcodes;

        var sortColumn = sort.Trim();
        var sortDirection = "asc";

        if (sortColumn.StartsWith('-'))
        {
            sortDirection = "desc";
            sortColumn = sortColumn.Substring(1);
        }

        var type = typeof(StreetcodeContent);
        var property = type.GetProperty(sortColumn);
        if (property is null)
        {
            return Result.Fail(new Error("CannotFindAnyPropertyWithThisName"));
        }

        var parameter = Expression.Parameter(type, "p");
        var memberExpression = Expression.Property(parameter, sortColumn);
        var lambda = Expression.Lambda(memberExpression, parameter);

        streetcodes = sortDirection switch
        {
            "asc" => Queryable.OrderBy(sortedRecords, (dynamic)lambda),
            "desc" => Queryable.OrderByDescending(sortedRecords, (dynamic)lambda),
            _ => sortedRecords,
        };

        return Result.Ok();
    }

    private static void ApplyPagination(
        ref IQueryable<StreetcodeContent> streetcodes,
        int amount,
        int page)
    {
        streetcodes = streetcodes
            .Skip((page - 1) * amount)
            .Take(amount);
    }
}