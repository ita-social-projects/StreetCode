using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetById;

public class GetToponymByIdHandler : IRequestHandler<GetToponymByIdQuery, Result<ToponymDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;

    public GetToponymByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ToponymDTO>> Handle(GetToponymByIdQuery request, CancellationToken cancellationToken)
    {
        var toponym = await _repositoryWrapper.ToponymRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (toponym is null)
        {
            string errorMsg = $"Cannot find any toponym with corresponding id: {request.Id}";
            _logger.LogError(request, errorMsg);
            return Result.Fail(new Error(errorMsg));
        }

        return Result.Ok(_mapper.Map<ToponymDTO>(toponym));
    }
}