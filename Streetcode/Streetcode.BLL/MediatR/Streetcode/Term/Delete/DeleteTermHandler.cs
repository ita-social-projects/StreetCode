using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Term.Delete
{
    public class DeleteTermHandler : IRequestHandler<DeleteTermCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public DeleteTermHandler(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteTermCommand request, CancellationToken cancellationToken)
        {
            var term = await _repository.TermRepository.GetFirstOrDefaultAsync((term) => term.Id == request.id);

            if (term is null)
            {
                return Result.Fail(new Error("Cannot convert null to Term"));
            }

            _repository.TermRepository.Delete(term);

            var resultIsSuccess = await _repository.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to delete a term"));
        }
    }
}
