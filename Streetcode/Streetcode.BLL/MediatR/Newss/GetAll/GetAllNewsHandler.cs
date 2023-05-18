using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Newss.GetAll
{
    public class GetAllNewsHandler : IRequestHandler<GetAllNewsQuery, Result<IEnumerable<NewsDTO>>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;

        public GetAllNewsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<Result<IEnumerable<NewsDTO>>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _repositoryWrapper.NewsRepository.GetAllAsync(
                include: cat => cat.Include(img => img.Image));
            if (news == null)
            {
                return Result.Fail("There are no news in the database");
            }

            var newsDTOs = _mapper.Map<IEnumerable<NewsDTO>>(news);

            foreach (var dto in newsDTOs)
            {
                if(dto.Image is not null)
                {
                    dto.Image.Base64 = _blobService.FindFileInStorageAsBase64(dto.Image.BlobName);
                }
            }

            return Result.Ok(newsDTOs);
        }
    }
}
