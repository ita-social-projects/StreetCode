using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetById;

public class GetImageByIdHandler : IRequestHandler<GetImageByIdQuery, Result<ImageDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetImageByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<ImageDTO>> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
    {
        var image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (image is null)
        {
            return Result.Fail(new Error($"Cannot find a image with corresponding id: {request.Id}"));
        }

        var imageDto = _mapper.Map<ImageDTO>(image);

        imageDto.Base64 = _blobService.FindFileInStorageAsBase64(image.BlobName);

        return Result.Ok(imageDto);
    }
}