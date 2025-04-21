using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.GetAll
{
    public class GetAllTermsHandler : IRequestHandler<GetAllTermsQuery, Result<GetAllTermsResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllTermsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<GetAllTermsResponseDto>> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
        {
            var allTerms = await _repositoryWrapper.TermRepository.GetAllAsync(
                predicate: null,
                include: null);

            var filteredTerms = allTerms.ToList();

            if (!string.IsNullOrWhiteSpace(request.title))
            {
                filteredTerms = filteredTerms
                    .Where(t => t.Title != null && t.Title.ToLower().Contains(request.title.Trim().ToLower()))
                    .ToList();
            }

            var totalAmount = filteredTerms.Count;

            var paginatedTerms = filteredTerms
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToList();

            var termDTOs = _mapper.Map<IEnumerable<TermDTO>>(paginatedTerms);

            var response = new GetAllTermsResponseDto
            {
                TotalAmount = totalAmount,
                Terms = termDTOs
            };

            return Result.Ok(response);
        }
    }
}