using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create
{
    public class CreateTermHandler : IRequestHandler<CreateTermCommand, Result<TermDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CreateTermHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<TermDTO>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);

            if (term is null)
            {
                return Result.Fail(new Error("Cannot convert null to Term"));
            }

            var createdTerm = _repository.TermRepository.Create(term);

            if (createdTerm is null)
            {
                return Result.Fail(new Error("Cannot create term"));
            }

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;

            if(!resultIsSuccess)
            {
                return Result.Fail(new Error("Failed to create a term"));
            }

            var createdTermDTO = _mapper.Map<TermDTO>(createdTerm);

            return createdTermDTO != null ? Result.Ok(createdTermDTO) : Result.Fail(new Error("Failed to map created term"));
        }
    }
}
