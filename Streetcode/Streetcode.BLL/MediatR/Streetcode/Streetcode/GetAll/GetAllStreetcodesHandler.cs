using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;

public class GetAllStreetcodesHandler : IRequestHandler<GetAllStreetcodesQuery, Result<IEnumerable<StreetcodeDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllStreetcodesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<StreetcodeDTO>>> Handle(GetAllStreetcodesQuery request, CancellationToken cancellationToken)
    {
        var streetcodes = _repositoryWrapper.StreetcodeRepository
            .FindAll();

        if (request.Title is not null)
        {
            FindStreetcodesWithMatchTitle(ref streetcodes, request.Title);
        }

        if (request.Sort is not null)
        {
            FindSortedStreetcodes(ref streetcodes, request.Sort);
        }

        if (request.Filter is not null)
        {
            FindFilteredStreetcodes(ref streetcodes, request.Filter);
        }

        ApplyPagination(ref streetcodes, request.Amount, request.Page);

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(streetcodes.AsEnumerable());
        return Result.Ok(streetcodeDtos);
    }

    private void FindStreetcodesWithMatchTitle(
        ref IQueryable<StreetcodeContent> streetcodes,
        string title)
    {
        streetcodes = streetcodes.Where(s => s.Title
            .ToLower()
            .Contains(title
            .ToLower()) || s.Index
            .ToString() == title);
    }

    private void FindFilteredStreetcodes(
        ref IQueryable<StreetcodeContent> streetcodes,
        string filter)
    {
        var filterParams = filter.Split(':');
        var filterColumn = filterParams[0];
        var filterValue = filterParams[1];

        streetcodes = streetcodes
            .AsEnumerable()
            .Where(s => filterValue == s.Status.ToString())
            .AsQueryable();
    }

    private void FindSortedStreetcodes(
        ref IQueryable<StreetcodeContent> streetcodes,
        string sort)
    {
        var sortedRecords = streetcodes;

        var sortColumn = sort.Trim();
        var sortDirection = "asc";

        if (sortColumn.StartsWith("-"))
        {
            sortDirection = "desc";
            sortColumn = sortColumn.Substring(1);
        }

        var type = typeof(StreetcodeContent);
        var parameter = Expression.Parameter(type, "p");
        var property = Expression.Property(parameter, sortColumn);
        var lambda = Expression.Lambda(property, parameter);

        streetcodes = sortDirection switch
        {
            "asc" => Queryable.OrderBy(sortedRecords, (dynamic)lambda),
            "desc" => Queryable.OrderByDescending(sortedRecords, (dynamic)lambda),
            _ => sortedRecords,
        };
    }

    private void ApplyPagination(
        ref IQueryable<StreetcodeContent> streetcodes,
        int amount,
        int page)
    {
        var totalPages = (int)Math.Ceiling(streetcodes.Count() / (double)amount);

        streetcodes = streetcodes
            .Skip((page - 1) * amount)
            .Take(amount);
    }
}