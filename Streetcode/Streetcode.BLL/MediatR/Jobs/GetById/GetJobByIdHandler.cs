using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Jobs.GetById
{
    public class GetJobByIdHandler : IRequestHandler<GetJobByIdQuery, Result<JobDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _reppository;
        private readonly ILoggerService _loggerService;
        private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizer;

        public GetJobByIdHandler(IMapper mapper, IRepositoryWrapper reppository, ILoggerService loggerService, IStringLocalizer<CannotFindSharedResource> stringLocalizer)
        {
            _mapper = mapper;
            _reppository = reppository;
            _loggerService = loggerService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Result<JobDto>> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            var job = await _reppository.JobRepository.GetFirstOrDefaultAsync(j => j.Id == request.jobId);

            if (job is null)
            {
                string exceptionMessege = _stringLocalizer["CannotFindJobWithCorrespondingId", request.jobId];
                _loggerService.LogError(request, exceptionMessege);
                return Result.Fail(exceptionMessege);
            }

            try
            {
                var jobDto = _mapper.Map<JobDto>(job);
                return Result.Ok(jobDto);
            }
            catch (Exception ex)
            {
                _loggerService.LogError(request, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
