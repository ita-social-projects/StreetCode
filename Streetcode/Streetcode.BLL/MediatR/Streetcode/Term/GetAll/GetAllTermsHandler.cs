using System.Linq;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.TextContent;
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
            var termsQuery = _repositoryWrapper.TermRepository.FindAll();

            if (!string.IsNullOrWhiteSpace(request.title))
            {
                termsQuery = termsQuery.Where(t => t.Title != null && t.Title.ToLower().Contains(request.title.Trim().ToLower()));
            }

            var totalAmount = await termsQuery.CountAsync(cancellationToken);

            var paginatedTerms = await termsQuery
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync(cancellationToken);

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