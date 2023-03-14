using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Media.Image.UploadBase;

public class UploadBaseImageHandler : IRequestHandler<UploadBaseImageCommand, Result<Unit>>
{
    public UploadBaseImageHandler(IRepositoryWrapper repositoryWrapper)
    {
    }

    public async Task<Result<Unit>> Handle(UploadBaseImageCommand request, CancellationToken cancellationToken)
    {
        byte[] imageBytes = Convert.FromBase64String(request.Image.BaseFormat);

        string fileExtension = "." + request.Image.MimeType.Split('/')[1];

        string fileName = request.Image.Name + fileExtension;

        File.WriteAllBytes($"./StreetCode/BlobStorage/{fileName}", imageBytes);

        return Result.Ok(Unit.Value);
    }
}
