﻿using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Services.EntityAccessManager;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
  public class GetArtSlidesByStreetcodeIdHandler : IRequestHandler<GetArtSlidesByStreetcodeIdQuery, Result<IEnumerable<StreetcodeArtSlideDTO>>>
    {
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetArtSlidesByStreetcodeIdHandler(
            IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlobService blobService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<Result<IEnumerable<StreetcodeArtSlideDTO>>> Handle(GetArtSlidesByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<DAL.Entities.Streetcode.StreetcodeArtSlide, bool>>? basePredicate = sArtSlide => sArtSlide.StreetcodeId == request.StreetcodeId;
            var predicate = basePredicate.ExtendWithAccessPredicate(new StreetcodeAccessManager(), request.UserRole, sArtSlide => sArtSlide.Streetcode);

            var query = _repositoryWrapper.StreetcodeArtSlideRepository
                .FindAll(
                    predicate: predicate,
                    include: sArtSlide => sArtSlide
                        .Include(sArtSlide => sArtSlide.StreetcodeArts!)
                        .ThenInclude(sArt => sArt.Art)
                        .ThenInclude(art => art!.Image) !)
                .Skip((request.FromSlideN - 1) * request.AmoutOfSlides)
                .Take(request.AmoutOfSlides);

            var slides = await query.ToListAsync(cancellationToken);

            var slidesDto = _mapper.Map<IEnumerable<StreetcodeArtSlideDTO>>(slides);
            slidesDto = ConvertArtImagesToBase64(slidesDto);

            return Result.Ok(slidesDto);
        }

        private IEnumerable<StreetcodeArtSlideDTO> ConvertArtImagesToBase64(IEnumerable<StreetcodeArtSlideDTO> slidesDto)
        {
            foreach (var slideDto in slidesDto)
            {
                foreach (var artDto in slideDto.StreetcodeArts)
                {
                    if (artDto.Art.Image != null && artDto.Art.Image.BlobName != null)
                    {
                        artDto.Art.Image.Base64 = _blobService.FindFileInStorageAsBase64(artDto.Art.Image.BlobName);
                    }
                }
            }

            return slidesDto;
        }
    }
}
