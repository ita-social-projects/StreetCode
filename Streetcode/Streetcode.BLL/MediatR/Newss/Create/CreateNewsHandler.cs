using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Create
{
    public class CreateNewsHandler : IRequestHandler<CreateNewsCommand, Result<NewsDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        public CreateNewsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILoggerService logger)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
        }

        public async Task<Result<NewsDTO>> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var newNews = _mapper.Map<News>(request.newNews);
            if (newNews is null)
            {
                const string errorMsg = "Cannot convert null to news";
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            if (newNews.ImageId == 0)
            {
                newNews.ImageId = null;
            }

            var entity = _repositoryWrapper.NewsRepository.Create(newNews);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            if(resultIsSuccess)
            {
                return Result.Ok(_mapper.Map<NewsDTO>(entity));
            }
            else
            {
                const string errorMsg = "Failed to create a news";
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
