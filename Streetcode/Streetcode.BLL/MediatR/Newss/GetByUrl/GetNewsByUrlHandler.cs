﻿using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.EntityAccessManager;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.GetByUrl
{
    public class GetNewsByUrlHandler : IRequestHandler<GetNewsByUrlQuery, Result<NewsDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IBlobService _blobService;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetNewsByUrlHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IBlobService blobService, ILoggerService logger, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _blobService = blobService;
            _logger = logger;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<NewsDTO>> Handle(GetNewsByUrlQuery request, CancellationToken cancellationToken)
        {
            string url = request.Url;
            Expression<Func<News, bool>>? basePredicate = sc => sc.URL == url;
            var predicate = basePredicate.ExtendWithAccessPredicate(new NewsAccessManager(), request.UserRole);

            var newsDTO = _mapper.Map<NewsDTO>(await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(
                predicate: predicate,
                include: scl => scl
                    .Include(sc => sc.Image!)));

            if(newsDTO is null)
            {
                string errorMsg = _stringLocalizerNo["NoNewsByEnteredUrl", url].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(errorMsg);
            }

            if (newsDTO.Image is not null)
            {
                newsDTO.Image.Base64 = _blobService.FindFileInStorageAsBase64(newsDTO.Image.BlobName);
            }

            return Result.Ok(newsDTO);
        }
    }
}