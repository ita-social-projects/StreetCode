using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.Create
{
    public class CreateJobHandler : IRequestHandler<CreateJobCommand, Result<JobDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreateJobHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<JobDto>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var job = _mapper.Map<Job>(request.job);
                var createdJob = await _repositoryWrapper.JobRepository.CreateAsync(job);
                await _repositoryWrapper.SaveChangesAsync();
                return Result.Ok(_mapper.Map<JobDto>(createdJob));
            }
            catch (Exception ex)
            {
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
            }
        }
    }
}
