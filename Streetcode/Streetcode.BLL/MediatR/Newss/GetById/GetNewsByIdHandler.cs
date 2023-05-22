using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace Streetcode.BLL.MediatR.Newss.GetById
{
    public class GetNewsByIdHandler : IRequestHandler<GetNewsByIdQuery, Result<NewsDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        public GetNewsByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _blobService = blobService;
        }

        public async Task<Result<NewsDTO>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            int id = request.id;
            var newsDTO = _mapper.Map<NewsDTO>(await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(
                predicate: sc => sc.Id == id,
                include: scl => scl
                    .Include(sc => sc.Image)));
            if(newsDTO is null)
            {
                return Result.Fail($"No news by entered Id - {id}");
            }

            if (newsDTO.Image is not null)
            {
                newsDTO.Image.Base64 = _blobService.FindFileInStorageAsBase64(newsDTO.Image.BlobName);
            }

            return Result.Ok(newsDTO);
        }
    }
}