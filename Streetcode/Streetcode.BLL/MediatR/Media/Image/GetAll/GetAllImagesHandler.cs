using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.GetAll;

public class GetAllImagesHandler : IRequestHandler<GetAllImagesQuery, Result<IEnumerable<ImageDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllImagesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ImageDTO>>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _repositoryWrapper.ImageRepository.GetAllAsync();

        if (images is null)
        {
            return Result.Fail(new Error($"Cannot find any image"));
        }

        var imageDtos = _mapper.Map<IEnumerable<ImageDTO>>(images);
        return Result.Ok(imageDtos);
    }
}