using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, Result<UpdateNewsDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobSevice;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<CannotConvertNullSharedResource> _stringLocalizerCannotConvertNull;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        public UpdateNewsHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService,
            ILoggerService logger,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            IStringLocalizer<CannotConvertNullSharedResource> stringLocalizerCannotConvertNull)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobSevice = blobService;
            _logger = logger;
            _stringLocalizerFailedToUpdate = stringLocalizerFailedToUpdate;
            _stringLocalizerCannotConvertNull = stringLocalizerCannotConvertNull;
        }

        public async Task<Result<UpdateNewsDTO>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<News>(request.news);
            if (news is null)
            {
                string errorMsg = _stringLocalizerCannotConvertNull["CannotConvertNullToNews"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            if (news.ImageId == 0)
            {
                string errorMsg = _stringLocalizerFailedToUpdate["Invalid imageId value"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            _repositoryWrapper.NewsRepository.Update(news);

            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if(resultIsSuccess)
            {
                return Result.Ok(_mapper.Map<UpdateNewsDTO>(news));
            }
            else
            {
                string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateNews"].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
