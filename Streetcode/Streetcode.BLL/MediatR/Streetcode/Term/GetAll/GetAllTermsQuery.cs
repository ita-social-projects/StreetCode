using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll;

public record GetAllTermsQuery(string? title = null, int page = 1, int pageSize = 10)
    : IRequest<Result<GetAllTermsDto>>;