using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
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
            streetcodes = streetcodes.Where(s => s.Title
            .ToLower()
            .Contains(request.Title
            .ToLower()) || s.Index
            .ToString() == request.Title);
        }

        if (request.Filter is not null)
        {
            var filterParams = request.Filter.Split(':');
            var filterColumn = filterParams[0];
            var filterValue = filterParams[1];

            streetcodes = streetcodes.Where(s => filterValue.Contains(s.Status.ToString()));
        }

        if (request.Sort is not null)
        {
            var sortParams = request.Sort.Split(',');
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

                sortedRecords = sortDirection switch
                {
                    "asc" => sortedRecords.OrderBy(s => property.GetValue(s, null)),
                    "desc" => sortedRecords.OrderByDescending(s => property.GetValue(s, null)),
                    _ => sortedRecords,
                };
            }

            streetcodes = sortedRecords;
        }

        var totalPages = (int)Math.Ceiling(streetcodes.Count() / (double)request.Amount);

        streetcodes = streetcodes.ToList()
            .Skip((request.Page - 1) * request.Amount)
            .Take(request.Amount);

        var streetcodeDtos = _mapper.Map<IEnumerable<StreetcodeDTO>>(streetcodes);
        return Result.Ok(streetcodeDtos);
    }
}