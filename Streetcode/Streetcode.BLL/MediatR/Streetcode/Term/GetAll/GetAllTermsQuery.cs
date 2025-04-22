using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll;

public record GetAllTermsQuery(string? title = null, int page = 1, int pageSize = 10)
    : IRequest<Result<GetAllTermsResponseDto>>;