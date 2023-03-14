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
        var streetcodes = await _repositoryWrapper.StreetcodeRepository
            .GetAllAsync();

        if (streetcodes is null)
        {
            return Result.Fail(new Error($"Cannot find any streetcodes"));
        }

        if (request.Title is not null)
        {
            FindStreetcodesWithMatchTitle(ref streetcodes, request.Title);
        }

        if (request.Filter is not null)
        {
            FindFilteredStreetcodes(ref streetcodes, request.Filter);
        }

        if (request.Sort is not null)
        {
            FindSortedStreetcodes(ref streetcodes, request.Sort);
        }

        var streetcodesList = streetcodes.AsEnumerable();

        foreach (var item in streetcodesList)
        {
            Console.WriteLine(item.Title);
        }

        ApplyPagination(ref streetcodesList, request.Amount, request.Page);

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(streetcodesList);
        return Result.Ok(streetcodeDtos);
    }

    private void FindStreetcodesWithMatchTitle(
        ref IEnumerable<StreetcodeContent> streetcodes,
        string title)
    {
        streetcodes = streetcodes.Where(s => s.Title
            .ToLower()
            .Contains(title
            .ToLower()) || s.Index
            .ToString() == title);
    }

    private void FindFilteredStreetcodes(
        ref IEnumerable<StreetcodeContent> streetcodes,
        string filter)
    {
        var filterParams = filter.Split(':');
        var filterColumn = filterParams[0];
        var filterValue = filterParams[1];

        streetcodes = streetcodes.Where(s => filterValue.Contains(s.Status.ToString()));
    }

    private void FindSortedStreetcodes(
        ref IEnumerable<StreetcodeContent> streetcodes,
        string sort)
    {
        var sortParams = sort.Split(',');
        var sortedRecords = streetcodes;

        foreach (var sortParam in sortParams)
        {
            var sortColumn = sortParam.Trim();
            var sortDirection = "asc";

            if (sortColumn.StartsWith("-"))
            {
                sortDirection = "desc";
                sortColumn = sortColumn.Substring(1);
            }

            var property = typeof(StreetcodeContent).GetProperty(sortColumn);

            streetcodes = sortDirection switch
            {
                "asc" => sortedRecords.OrderBy(s => property.GetValue(s, null)),
                "desc" => sortedRecords.OrderByDescending(s => property.GetValue(s, null)),
                _ => sortedRecords,
            };
        }
    }

    private void ApplyPagination(
        ref IEnumerable<StreetcodeContent> streetcodes,
        int amount,
        int page)
    {
        var totalPages = (int)Math.Ceiling(streetcodes.Count() / (double)amount);

        streetcodes = streetcodes
            .Skip((page - 1) * amount)
            .Take(amount);
    }
}