using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Update
{
    public class UpdateRelatedTermHandler : IRequestHandler<UpdateRelatedTermCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerService _logger;

        public UpdateRelatedTermHandler(IMapper mapper, IRepositoryWrapper repository, ILoggerService logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public Task<Result<Unit>> Handle(UpdateRelatedTermCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
