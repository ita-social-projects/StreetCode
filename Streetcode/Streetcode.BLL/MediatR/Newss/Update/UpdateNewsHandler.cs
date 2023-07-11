using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, Result<NewsDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobSevice;
        private readonly ILoggerService _logger;
        public UpdateNewsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobSevice = blobService;
            _logger = logger;
        }

        public async Task<Result<NewsDTO>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<News>(request.news);
            if (news is null)
            {
                const string errorMsg = $"Cannot convert null to news";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var response = _mapper.Map<NewsDTO>(news);

            if (news.Image is not null)
            {
                response.Image.Base64 = _blobSevice.FindFileInStorageAsBase64(response.Image.BlobName);
            }
            else
            {
                var img = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(x => x.Id == response.ImageId);
                if (img != null)
                {
                    _repositoryWrapper.ImageRepository.Delete(img);
                }
            }

            _repositoryWrapper.NewsRepository.Update(news);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

            if(resultIsSuccess)
            {
                return Result.Ok(response);
            }
            else
            {
                const string errorMsg = $"Failed to update news";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
