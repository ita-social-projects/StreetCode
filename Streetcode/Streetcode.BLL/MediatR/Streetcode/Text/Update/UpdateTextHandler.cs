using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.MediatR.Streetcode.Text.Update;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Fact.Update;

public class UpdateTextHandler : IRequestHandler<UpdateTextCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdateTextHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(UpdateTextCommand request, CancellationToken cancellationToken)
    {
        var text = _mapper.Map<DAL.Entities.Streetcode.TextContent.Text>(request.Text);

        if (text is null)
        {
            return Result.Fail(new Error("Cannot convert null to Text"));
        }

        _repositoryWrapper.TextRepository.Update(text);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to update a text"));
    }
}