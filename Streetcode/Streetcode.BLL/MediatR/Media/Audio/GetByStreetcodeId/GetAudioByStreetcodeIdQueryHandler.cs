using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;

public class GetAudioByStreetcodeIdQueryHandler : IRequestHandler<GetAudioByStreetcodeIdQuery, Result<AudioDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    public GetAudioByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IBlobService blobService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<Result<AudioDTO>> Handle(GetAudioByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(
            s => s.Id == request.StreetcodeId,
            include: q => q.Include(s => s.Audio) !);
        if (streetcode == null)
        {
            return Result.Fail(new Error($"Cannot find an audio with the corresponding streetcode id: {request.StreetcodeId}, because no streetcode with such streetcode exist"));
        }

        AudioDTO audioDto = _mapper.Map<AudioDTO>(streetcode.Audio);
        if (streetcode.Audio != null)
        {
            audioDto.Base64 = _blobService.FindFileInStorageAsBase64(audioDto.BlobName);
        }

        NullResult<AudioDTO> result = new NullResult<AudioDTO>();
        result.WithValue(_mapper.Map<AudioDTO>(streetcode.Audio));
        return result;
    }
}