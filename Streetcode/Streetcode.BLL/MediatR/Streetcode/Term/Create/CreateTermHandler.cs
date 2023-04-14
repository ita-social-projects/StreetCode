using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Create
{
    public class CreateTermHandler : IRequestHandler<CreateTermCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CreateTermHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(CreateTermCommand request, CancellationToken cancellationToken)
        {
            var term = _mapper.Map<DAL.Entities.Streetcode.TextContent.Term>(request.Term);

            if (term is null)
            {
                return Result.Fail(new Error("Cannot convert null to Term"));
            }

            _repository.TermRepository.Create(term);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a term"));
        }
    }
}
