using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Toponyms.GetById;

public class GetToponymByIdHandler : IRequestHandler<GetToponymByIdQuery, Result<ToponymDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetToponymByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<ToponymDTO>> Handle(GetToponymByIdQuery request, CancellationToken cancellationToken)
    {
        var toponym = await _repositoryWrapper.ToponymRepository
            .GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (toponym is null)
        {
            return Result.Fail(new Error($"Cannot find any toponym with corresponding id: {request.Id}"));
        }

        var toponymDto = _mapper.Map<ToponymDTO>(toponym);
        return Result.Ok(toponymDto);
    }
}