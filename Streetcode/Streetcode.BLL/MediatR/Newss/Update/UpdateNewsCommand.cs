using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public record UpdateNewsCommand(UpdateNewsDto news)
        : IRequest<Result<UpdateNewsDto>>;
}
