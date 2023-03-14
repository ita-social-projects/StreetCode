using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.BLL.MediatR.Media.Image.GetBaseFile;

public class GetBaseFileByNameHandler : IRequestHandler<GetBaseFileByNameQuery, Result<ImageBaseDTO>>
{
    private readonly IBlobService _blobStorage;

    public GetBaseFileByNameHandler(IBlobService blobService)
    {
        _blobStorage = blobService;
    }

    public async Task<Result<ImageBaseDTO>> Handle(GetBaseFileByNameQuery request, CancellationToken cancellationToken)
    {
        string filePath = $"../../BlobStorage/{request.Name}";

        string mimeType = request.Name.Split('.')[1];

        var files = Convert.ToBase64String(File.ReadAllBytes(filePath));

        var result = new ImageBaseDTO()
        {
            Name = request.Name,
            BaseFormat = files,
            MimeType = mimeType
        };

        return Result.Ok(result);
    }
}
