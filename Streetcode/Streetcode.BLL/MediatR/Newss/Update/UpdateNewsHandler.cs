using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, Result<NewsDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobSevice;
        public UpdateNewsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobSevice = blobService;
        }

        public async Task<Result<NewsDTO>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<News>(request.news);
            if (news is null)
            {
                return Result.Fail(new Error("Cannot convert null to news"));
            }

            var response = _mapper.Map<NewsDTO>(news);

            if (news.Image is not null)
            {
                response.Image.Base64 = _blobSevice.FindFileInStorageAsBase64(response.Image.BlobName);
            }
            else
            {
                var img = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(x => x.Id == response.ImageId);
                response.Image = null;
                _repositoryWrapper.ImageRepository.Delete(img);
            }

            _repositoryWrapper.NewsRepository.Update(news);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(response) : Result.Fail(new Error("Failed to update news"));
        }
    }
}
